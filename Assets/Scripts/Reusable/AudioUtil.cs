using UnityEngine;
using System.Collections;

/// <summary>
/// Sample Audio Util methods.
/// </summary>
public static class AudioUtil {
	/// <summary>
	/// A sample method for play the SE. 
	/// </summary>
	/// <param name="se">SE</param>
	/// <param name="delay">delay in seconds</param>
	/// <param name="loop">If uses loop</param>
	/// <returns>AudioSource used</returns>
	public static AudioSource PlaySE(AudioClip se, float delay=0, bool loop=false){
		AudioSource audioSource = Camera.main.gameObject.AddComponent<AudioSource>();
		audioSource.clip = se;
		audioSource.loop = loop;
		audioSource.PlayDelayed(delay);
		if(!loop)
			Object.Destroy (audioSource, delay+se.length);
		return audioSource;
	}
}