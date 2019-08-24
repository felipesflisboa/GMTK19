using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

/// <summary>
/// Handle used commands on a stage.
/// </summary>
public class CommandHandler {
	Dictionary<string, bool> used = new Dictionary<string, bool>();
	
	public int NewCommandsUsedThisFrame{
		get {
			return used.Keys.Count((k) => Input.GetButtonUp(k) && !used[k]);
		}
	}

	public void Setup(string[] keyArray) {
		foreach (var s in keyArray)
			used.Add(s, false);
	}

	public bool HasUsed(string command) {
		Assert.IsTrue(used.ContainsKey(command), string.Format("Invalid command {0}.", command));
		return used[command];
	}

	public void RefreshCommandsUsed() {
		foreach (var k in new List<string>(used.Keys)) //TODO, look alloc
			if (Input.GetButtonUp(k) && !used[k])
				used[k] = true;
	}

	public bool GetButtonValid(string key) {
		return !used[key] && Input.GetButton(key);
	}

	public bool GetButtonDownValid(string key) {
		return !used[key] && Input.GetButtonDown(key);
	}
}
