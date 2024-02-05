using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] Transform camRoot;
    [SerializeField] Transform moveRoot;
    [SerializeField] float sensivility = 10f;
    [SerializeField] float characterRotationSpeed = 10f;
    [SerializeField] Transform aimPoint;
    [SerializeField] bool follow = true;

    public Transform AimPoint { get { return aimPoint; } }

    Vector2 lookInput;
    float lastLookDistSqr;
    float yAngle;
    float xAngle;
    LayerMask environmentMask;

    private void Awake()
    {
        float height = GetComponent<CharacterController>().height;
        environmentMask = LayerMask.GetMask("Environment");
        lastLookDistSqr = 50f;
        //GameManager.Instance.OnFocus.AddListener(AutoEnable);
    }

	private void OnDestroy()
	{
		//GameManager.Instance.OnFocus.RemoveListener(AutoEnable);
	}

	private void AutoEnable(bool focus)
    {
        enabled = focus;
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        bool result = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
            out RaycastHit hitInfo, 100f, environmentMask);

        Vector3 lookPoint;
        if(true == result && (hitInfo.point - transform.position).sqrMagnitude > 3f * 3f)
        {
            lookPoint = hitInfo.point;
            lastLookDistSqr = (lookPoint - Camera.main.transform.position).sqrMagnitude;
        }
        else
        {
            lookPoint = Camera.main.transform.position + Camera.main.transform.forward * Mathf.Sqrt(lastLookDistSqr);
        }

        aimPoint.transform.position = Vector3.Lerp(aimPoint.transform.position, lookPoint,
           characterRotationSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Look()
    {
        if (follow == false) return;
        xAngle += lookInput.x * Time.deltaTime * sensivility;
        yAngle += lookInput.y * Time.deltaTime * sensivility;
        yAngle = Mathf.Clamp(yAngle, -50f, 50f);

        moveRoot.rotation = Quaternion.Euler(new Vector3(0f, xAngle, 0f));
		camRoot.rotation = Quaternion.Euler(new Vector3(-yAngle, xAngle, 0f));
    }

    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
}
