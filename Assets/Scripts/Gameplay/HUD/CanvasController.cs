using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
	[SerializeField] GameObject tipPrefab;
	[SerializeField] RectTransform tipRectTransform;
	[SerializeField] Text timeLabel;
	[SerializeField] Slider[] timeSliderArray;
	RectTransform rectTransform;
	Canvas canvas;
	CanvasScaler canvasScaler;
	List<TipIcon> tipIconList = new List<TipIcon>();

	const float REMAINING_TIME_TO_START_FLASHING_EFFECT = 8f;

	public bool ShowingTips {
		get {
			return tipIconList.Count > 0;
		}
	}

	/// <summary>
	/// Extra value to be used on camera to get the right zoom value.
	/// </summary>
	public float HeightScaleRatio {
		get {
			return rectTransform.rect.height / canvasScaler.referenceResolution.y;
		}
	}

	void Awake() {
		DontDestroyOnLoad(gameObject);
		rectTransform = (RectTransform)transform;
		canvas = GetComponent<Canvas>();
		canvasScaler = canvas.GetComponent<CanvasScaler>();
	}

	void OnEnable() {
		StartCoroutine(TimeRefreshRoutine());
	}

	void Update() {
		if (!ShowingTips && GameManager.I.currentStage.CanShowTips)
			StartCoroutine(ShowTipsRoutine(GameManager.I.currentStage.tipSpriteArray));
		RefreshScalerMatch();
	}

	void RefreshScalerMatch() {
		if (rectTransform.rect.width < canvasScaler.referenceResolution.x && canvasScaler.matchWidthOrHeight == 1)
			canvasScaler.matchWidthOrHeight = 0;
		if (rectTransform.rect.height < canvasScaler.referenceResolution.y && canvasScaler.matchWidthOrHeight == 0)
			canvasScaler.matchWidthOrHeight = 1;
	}

	IEnumerator TimeRefreshRoutine() {
		StartCoroutine(FlashingTimeEffect());
		while (true) {
			timeLabel.text = Mathf.RoundToInt(GameManager.I.ClampedRemainingTime).ToString("00");
			foreach (var slider in timeSliderArray)
				slider.value = GameManager.I.RemainingTimeRatio;
			yield return new WaitForSeconds(0.14f);
		}
	}

	IEnumerator FlashingTimeEffect() {
		while(true) {
			if(timeLabel.gameObject.activeSelf)
				yield return new WaitWhile(() => GameManager.I.RemainingTime > REMAINING_TIME_TO_START_FLASHING_EFFECT);
			timeLabel.gameObject.SetActive(!timeLabel.gameObject.activeSelf);
			yield return new WaitForSeconds(0.25f);
		}
	}

	IEnumerator ShowTipsRoutine(Sprite[] tipSpriteArray) {
		foreach (var sprite in tipSpriteArray)
			tipIconList.Add(CreateTipIcon(sprite));
		foreach (var tipIcon in tipIconList)
			yield return tipIcon.StartCoroutine(tipIcon.PlayAnimation());
	}

	TipIcon CreateTipIcon(Sprite sprite) {
		TipIcon ret = Instantiate(tipPrefab, tipRectTransform).GetComponent<TipIcon>();
		ret.Setup(sprite);
		return ret;
	}

	public void ClearTips() {
		StartCoroutine(ClearTipsRoutine());
	}

	IEnumerator ClearTipsRoutine() {
		yield return null; // For when loading another stage
		foreach (var tipIcon in tipIconList)
			Destroy(tipIcon.gameObject);
		tipIconList.Clear();
	}
}
