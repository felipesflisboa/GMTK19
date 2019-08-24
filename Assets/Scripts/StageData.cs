using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageData : ScriptableObject {
	public SceneReference[] array;

	public int FindIndex(string path) {
		return System.Array.IndexOf(array, array.First((sr) => sr.ScenePath == path));
	}

	public bool IsLast(string path) {
		return FindIndex(path) == array.Length - 1;
	}

	public SceneReference GetNext(string path) {
		return array[FindIndex(path) + 1];
	}
}
