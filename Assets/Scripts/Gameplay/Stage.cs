using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {
	public Transform startPoint;
	public bool cameraFollow;
	public float startTime { get; private set; }

	[Header("Tips")]
	public Sprite[] tipSpriteArray = new Sprite[0];
	public float secondsRemainingToShowTips;

	public bool CanShowTips => tipSpriteArray.Length > 0 && secondsRemainingToShowTips > GameManager.I.RemainingTime;

	public void Awake() {
		GameManager.I.InitializeStage(this);
		startTime = GameManager.I.elapsedTime;
	}
}
