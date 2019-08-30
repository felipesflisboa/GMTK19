using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class KeyIcon : MonoBehaviour {
	[SerializeField] string command;
	[SerializeField] Sprite availableSprite;
	[SerializeField] Sprite activeSprite;
	[SerializeField] Sprite unavailableSprite;
	Image icon;

	protected virtual bool Active => activeSprite != null && Input.GetButton(command);
	protected virtual bool Unavailable => GameManager.I.player.commandHandler.HasUsed(command);

	void Awake () {
		icon = GetComponent<Image>();
	}
	
	void LateUpdate () {
		RefreshSprite();
	}

	void RefreshSprite() {
		if (Unavailable) {
			icon.sprite = unavailableSprite;
		} else if (Active) {
			icon.sprite = activeSprite;
		} else {
			icon.sprite = availableSprite;
		}
	}
}
