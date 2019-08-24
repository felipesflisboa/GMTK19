using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BombExplosion : MonoBehaviour {
	IEnumerator Start() {
		yield return transform.DOScale(0.05f, 0.4f).From().SetEase(Ease.OutCubic).WaitForCompletion();
		yield return new WaitForSeconds(0.1f);
		StopCausingDamage();
		yield return transform.GetComponent<Renderer>().material.DOColor(Color.clear, 0.5f).SetEase(Ease.OutQuad).WaitForCompletion();
		Destroy(gameObject);
	}

	void StopCausingDamage() {
		foreach (Collider col in GetComponentsInChildren<Collider>(true))
			col.enabled = false;
	}

	void OnTriggerEnter(Collider other) {
		IDestructible destructible = other.GetComponentInParent<IDestructible>();
		if (destructible != null)
			destructible.Destroy();
	}
}
