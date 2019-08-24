﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
	[SerializeField] Text timeLabel;
	[SerializeField] Slider[] timeSliderArray;

	void Awake() {
		DontDestroyOnLoad(gameObject);	
	}

	void Update() {
		timeLabel.text = Mathf.RoundToInt(GameManager.I.TimeRemainingClamped).ToString("00");
		foreach (var slider in timeSliderArray)
			slider.value = GameManager.I.TimeRemainingRatio;
	}
}