using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;
using DG.Tweening;

public class CameraController : MonoBehaviour {
	void Start() {
		ExecuteInitialAnimation(GameManager.INITIAL_SETUP_TIME, 0.1f);
		RenderSettings.fog = true;
	}

	void ExecuteInitialAnimation(float duration, float movementDelay) {
		if (GameManager.I.currentStage.cameraFollow)
			transform.position = transform.position.WithX(GameManager.I.player.transform.position.x);
		transform.DOMove(
			GameManager.I.player.transform.position + Vector3.back * 3.7f, duration - movementDelay
		).From().SetEase(Ease.InSine).SetDelay(movementDelay);
		transform.DORotateQuaternion(Quaternion.identity, duration).From().SetEase(Ease.InSine);
	}

	void LateUpdate() {
		if (GameManager.I.currentStage.cameraFollow)
			transform.position = transform.position.WithX(PickCurrentPosX());
		UpdateFog();
	}

	float PickCurrentPosX() {
		return Mathf.Lerp(transform.position.x, GameManager.I.player.transform.position.x, 5 * Time.deltaTime);
	}

	void UpdateFog() {
		RenderSettings.fogStartDistance = GameManager.I.player.transform.position.z - transform.position.z - 6;
		RenderSettings.fogEndDistance = RenderSettings.fogStartDistance + 11;
	}
}
