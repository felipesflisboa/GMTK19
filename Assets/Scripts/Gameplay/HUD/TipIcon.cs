using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TipIcon : MonoBehaviour {
	Image image;
	public bool useSprite { get; private set; }

	void Awake() {
		image = GetComponent<Image>();
	}

	public void Setup(Sprite sprite) {
		useSprite = sprite != null;
		if(useSprite)
			image.sprite = sprite;
		image.color = Color.clear;
	}

	public IEnumerator PlayAnimation() {
		if(!useSprite)
			yield break;
		for (int i = 0; i < 5; i++) {
			if (image)
				image.color = Color.clear;
			yield return new WaitForSeconds(0.05f);
			if (image)
				image.color = Color.white;
			yield return new WaitForSeconds(0.05f);
		}
	}
}
