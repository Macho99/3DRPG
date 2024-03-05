using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

// https://bonnate.tistory.com/221 참고용 자료

public class HeadAiming : MonoBehaviour
{
    [Tooltip("프로시저 애니메이션을 시작하는 거리")]
    [SerializeField] private float mProcedureAnimDistance;

    [Tooltip("프로시저 애니메이션 컨트롤러를 담는 부모 트랜스폼")]
    [SerializeField] private Transform mProcedureAnimParent;

    private Transform mProcedureAnimBaseTaget; // 기본 타겟 (배열 0번)

    private MultiAimConstraint[] mProcedureAnimControllers; // 컨트롤러들

    private float[] mProcedureAnimOriginWeights; // 컨트롤러들의 초기 가중치 값

    public bool mIsLookingTarget;  // 대상을 보고있는가

    private bool mIsBackwardWeightCorReady; // 바라보게 하지 않는 코루틴이 실행 가능 상태인가?

    private bool mIsForceLookTarget;    // 어떠한 대상을 무조건적으로 바라보는가?

    [HideInInspector] public bool IsEnableTargetLook = true; // 대상을 바라보는 것을 활성화 하는가?

    private int ControllerLength // 컨트롤러의 개수
    {
        get
        {
            return mProcedureAnimControllers.Length;
        }
    }

    private Coroutine mWeightCoroutine, mLookAtCoroutine;

    private void Awake()
    {
        mProcedureAnimControllers = mProcedureAnimParent.GetComponentsInChildren<MultiAimConstraint>();

        mProcedureAnimBaseTaget = mProcedureAnimControllers[0].data.sourceObjects.GetTransform(0);

        mProcedureAnimOriginWeights = new float[ControllerLength];

        for(int i = 0; i < ControllerLength; i++)
        {
            mProcedureAnimOriginWeights[i] = mProcedureAnimControllers[i].weight;
            mProcedureAnimControllers[i].weight = 0f;
        }

        mIsForceLookTarget = mIsBackwardWeightCorReady = mIsLookingTarget = false;
        IsEnableTargetLook = true;
    }

    private void Update()
    {
        if (IsEnableTargetLook && (transform.position - mProcedureAnimBaseTaget.position).magnitude < mProcedureAnimDistance || mIsForceLookTarget)
        {
            if (mIsLookingTarget) { return; }

            mIsBackwardWeightCorReady = mIsLookingTarget = true;

            LookBaseTarget(true);
        }
        else
        {
            if (!mIsBackwardWeightCorReady) { return; }
            mIsBackwardWeightCorReady = false;
            LookBaseTarget(false);
        }
    }


    private void LookBaseTarget(bool isForward)
    {
        if(mWeightCoroutine != null) { StopCoroutine(mWeightCoroutine); }
        mWeightCoroutine = StartCoroutine(COR_FadeWeight(isForward));
    }

    public void LookTarget(Transform targetTrans, float duration = 3.0f, float lookTransitionSpeed = 1.0f, float releaseTransitionSpeed = 5.0f)
    {
        if (mLookAtCoroutine != null) { StopCoroutine(mLookAtCoroutine); }
        mLookAtCoroutine = StartCoroutine(COR_LookTargetLerp(targetTrans, duration, lookTransitionSpeed, releaseTransitionSpeed));
    }


    private IEnumerator COR_LookTargetLerp(Transform targetTrans, float duration, float lookTransitionSpeed, float releaseTransitionSpeed )
    {
        mIsForceLookTarget = true;

        WeightedTransformArray[] weightArray = new WeightedTransformArray[ControllerLength];

        float[] currentBaseWeightArray = new float[ControllerLength];
        float[] currentTargetWeightArray = new float[ControllerLength];
        bool isTargetAvailable = false;

        for (int i = 0; i < ControllerLength; i++)
        {
            weightArray[i] = mProcedureAnimControllers[i].data.sourceObjects;
            currentBaseWeightArray[i] = (weightArray[i].GetWeight(0) == 1.0f) ? mIsLookingTarget ? 1 : 0 : weightArray[i].GetWeight(0);

            if (weightArray[i].Count >= 2)
            {
                isTargetAvailable = true;
                currentTargetWeightArray[i] = weightArray[i].GetWeight(1);
            }
            for (int j = weightArray[i].Count - 1; j >= 1; --j)
            {
                weightArray[i].RemoveAt(j);
            }

            weightArray[i].Add(new WeightedTransform(targetTrans, 0));
            weightArray[i].SetTransform(1, targetTrans);

            mProcedureAnimControllers[i].data.sourceObjects = weightArray[i];
        }

        GetComponent<RigBuilder>().Build();

        // 진행도 생성 (가중치 반전)
        float process = 0f;

        while(process < 1f)
        {
            process += Time.deltaTime / lookTransitionSpeed;
            for (int i = 0; i < ControllerLength; ++i)
            {
                weightArray[i].SetWeight(0, Mathf.Lerp(currentBaseWeightArray[i], 0f, process));
                weightArray[i].SetWeight(1, Mathf.Lerp(isTargetAvailable ? currentTargetWeightArray[i] : 0f, 1f, process));

                mProcedureAnimControllers[i].data.sourceObjects = weightArray[i];
            }

            yield return null;
        }

        yield return new WaitForSeconds(duration);

        // 진행도 초기화 (가중치 초기 상태로 되돌리기)
        process = 0f;

        while (process < 1f)
        {
            process += Time.deltaTime / releaseTransitionSpeed;

            for (int i = 0; i < ControllerLength; ++i)
            {
                weightArray[i].SetWeight(0, Mathf.Lerp(0f, mIsLookingTarget ? 1f : 0f, process));
                weightArray[i].SetWeight(1, Mathf.Lerp(1f, 0f, process));

                mProcedureAnimControllers[i].data.sourceObjects = weightArray[i];
            }
            yield return null;
        }

        for (int i = 0; i < ControllerLength; ++i)
        {
            weightArray[i].RemoveAt(1);

            mProcedureAnimControllers[i].data.sourceObjects = weightArray[i];
        }

        mIsForceLookTarget = false;
    }

    private IEnumerator COR_FadeWeight(bool isForward)
    {
        // isForward가 false인 경우 바라보는중 해제 플래그
        if (!isForward) { mIsLookingTarget = false; }

        float[] currentProcedureAnimWeights = new float[ControllerLength];
        float process = 0f;

        for (int i = 0; i < ControllerLength; ++i) { currentProcedureAnimWeights[i] = mProcedureAnimControllers[i].weight; }
        while (true)
        {
            process += Time.deltaTime;
            for(int i = 0;i < ControllerLength; ++i) { mProcedureAnimControllers[i].weight = Mathf.Lerp(currentProcedureAnimWeights[i], isForward ? mProcedureAnimOriginWeights[i] : 0f, process); }

            if (process > 1f)
            {
                yield break;
            }

            yield return null;
        }
    }
}
