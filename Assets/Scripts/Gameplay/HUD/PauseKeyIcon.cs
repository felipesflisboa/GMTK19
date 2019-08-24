﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PauseKeyIcon : KeyIcon {
	protected override bool Active {
		get {
			return base.Active || Time.timeScale == 0;
		}
	}
}
