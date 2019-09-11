using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Always maintain Canvas Scaler min size (both width and height).
/// This way, black borders can be used on both axis.
/// </summary>
[RequireComponent(typeof(CanvasScaler))]
public class MinScreenResizer : MonoBehaviour {
	RectTransform rectTransform;
	CanvasScaler canvasScaler;

	void Awake() {
		rectTransform = (RectTransform)transform;
		canvasScaler = GetComponent<CanvasScaler>();
	}

	void Update() {
		if (rectTransform.rect.width < canvasScaler.referenceResolution.x && canvasScaler.matchWidthOrHeight == 1)
			canvasScaler.matchWidthOrHeight = 0;
		if (rectTransform.rect.height < canvasScaler.referenceResolution.y && canvasScaler.matchWidthOrHeight == 0)
			canvasScaler.matchWidthOrHeight = 1;
	}
}
