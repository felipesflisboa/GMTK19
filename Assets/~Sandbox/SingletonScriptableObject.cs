using System.Linq;
using UnityEngine;

/// <summary>
/// Abstract class for making reload-proof singletons out of ScriptableObjects
/// Returns the asset created on the editor, or null if there is none
/// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
/// 
/// This is important: before using the asset you must have a public variable in any script being used on 
/// the scene and associate manually the ScriptableObject to this variable; if you don't do that, it will
/// work only inside the editor. This is necessary because of how Unity deals with assets on runtime,
/// ignoring the ones with no references. We could also create a GameObject which only function is hold
/// a script full of public variables, each one with a ScriptableObject associated, and save it as a
/// prefab for reuse in other scenes.
/// </summary>
/// <typeparam name="T">Singleton type</typeparam>
public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject {
	static T _instance = null;
	public static T Instance {
		get {
			if (!_instance)
				_instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
			return _instance;
		}
	}
	public static T I {
		get {
			return Instance;
		}
	}
}
