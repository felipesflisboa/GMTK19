using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
	[SerializeField] GameObject tipPrefab;
	[SerializeField] RectTransform tipRectTransform;
	[SerializeField] Text timeLabel;
	[SerializeField] Slider[] timeSliderArray;
	List<Image> tipImageList = new List<Image>();

	public bool ShowingTips {
		get {
			return tipImageList.Count > 0;
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

	public IEnumerator ShowTipsRoutine(Sprite[] tipSpriteArray) {
		foreach (Sprite sprite in tipSpriteArray) {
			Image image = Instantiate(tipPrefab, tipRectTransform).GetComponent<Image>();
			image.color = Color.clear;
			if (sprite != null) {
				image.sprite = sprite;
				tipImageList.Add(image);
			}
		}
		//TODO destroy
		foreach (Image image in tipImageList) {
			for (int i = 0; i < 5; i++) {
				if(image)
					image.color = Color.clear;
				yield return new WaitForSeconds(0.05f);
				if (image)
					image.color = Color.white;
				yield return new WaitForSeconds(0.05f);
			}
		}
	}

	public void ClearTips() {
		StartCoroutine(ClearTipsRoutine());
	}

	IEnumerator ClearTipsRoutine() {
		yield return null; // For when loading another stage
		foreach (Image image in tipImageList)
			Destroy(image.gameObject);
		tipImageList.Clear();
	}
}
