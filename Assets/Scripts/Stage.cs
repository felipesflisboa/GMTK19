using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO remove singleton
public class Stage : SingletonMonoBehaviour<Stage> {
	public Transform startPoint;
	public bool cameraFollow;
	internal float startTime;

	public void Awake() {
		startTime = GameManager.I.timeElapsed;
		//TODO move
		Instantiate(GameManager.I.playerPrefab, startPoint.position, Quaternion.Euler(Vector3.up * 90));
		Camera.main.gameObject.AddComponent<CameraController>();
	}
}
