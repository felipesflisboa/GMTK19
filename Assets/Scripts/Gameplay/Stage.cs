using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {
	public Transform startPoint;
	public bool cameraFollow;
	public float startTime { get; private set; }

	public void Awake() {
		GameManager.I.InitializeStage(this);
		startTime = GameManager.I.elapsedTime;
	}
}
