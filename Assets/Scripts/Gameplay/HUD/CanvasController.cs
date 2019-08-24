using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
	[SerializeField] GameObject tipPrefab;
	[SerializeField] RectTransform tipRectTransform;
	[SerializeField] Text timeLabel;
	[SerializeField] Slider[] timeSliderArray;
	List<TipIcon> tipIconList = new List<TipIcon>();

	public bool ShowingTips {
		get {
			return tipIconList.Count > 0;
		}
	}

	void Awake() {
		DontDestroyOnLoad(gameObject);	
	}

	void OnEnable() {
		StartCoroutine(TimeRefreshRoutine());
	}

	float l;

	void Update() {
		if (!ShowingTips && GameManager.I.currentStage.CanShowTips)
			StartCoroutine(ShowTipsRoutine(GameManager.I.currentStage.tipSpriteArray));
	}

	IEnumerator TimeRefreshRoutine() {
		while (true) {
			timeLabel.text = Mathf.RoundToInt(GameManager.I.TimeRemainingClamped).ToString("00");
			foreach (var slider in timeSliderArray)
				slider.value = GameManager.I.TimeRemainingRatio;
			yield return new WaitForSeconds(0.14f);
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
