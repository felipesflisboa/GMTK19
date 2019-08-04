using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;
using DG.Tweening;

public class CameraController : MonoBehaviour {
	//TODO remove
	Vector3 initialPos;
	Quaternion initialRot;

	void Start() {
		if (Stage.I.cameraFollow)
			transform.position = transform.position.WithX(Player.I.transform.position.x);
		float delay = 0.1f; //TODO
		transform.DOMove(Player.I.transform.position + Vector3.back * 3.7f, Player.INITIAL_SETUP_TIME- delay).From().SetEase(Ease.InSine).SetDelay(0.1f);
		transform.DORotateQuaternion(Quaternion.identity, Player.INITIAL_SETUP_TIME).From().SetEase(Ease.InSine);
		RenderSettings.fog = true;
	}

	void LateUpdate() {
		if (Stage.I.cameraFollow)
			transform.position = transform.position.WithX(Mathf.Lerp(transform.position.x, Player.I.transform.position.x, 5 * Time.deltaTime));
		RenderSettings.fogStartDistance = Player.I.transform.position.z - transform.position.z - 6;
		RenderSettings.fogEndDistance = RenderSettings.fogStartDistance+11;
	}
}
