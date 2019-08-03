using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO require
public class KeyIcon : MonoBehaviour {
	[SerializeField] string command;
	Image icon;

	[SerializeField] Sprite availableSprite;
	[SerializeField] Sprite activeSprite;
	[SerializeField] Sprite unavailableSprite;

	void Awake () {
		icon = GetComponent<Image>();
	}
	
	void Update () {
		if (Player.I.commandsUsed[command]) {
			icon.sprite = unavailableSprite;
		}else if (activeSprite!=null && Input.GetButton(command)) {
			icon.sprite = activeSprite;
		} else {
			icon.sprite = availableSprite;
		}
	}
}
