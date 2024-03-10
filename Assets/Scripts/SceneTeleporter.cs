using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTeleporter : MonoBehaviour
{
	[SerializeField] string targetScene;
	[SerializeField] Transform targetTransform;

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			_ = StartCoroutine(CoTeleport());
		}
	}

	private IEnumerator CoTeleport()
	{
		FieldSFC.Instance.PlayFadeInAndOut();
		GameManager.UI.HideSceneUI(true);
		yield return new WaitForSeconds(0.5f);
		GameManager.UI.HideSceneUI(false, 2f);
		GameManager.Scene.MoveScene(targetScene);
		FieldSFC.Player.PlayerMove.Teleport(targetTransform.position);
	}
}
