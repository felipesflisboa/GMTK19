using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BombExplosion : MonoBehaviour {
	bool causeDamage = true;

	IEnumerator Start() {
		yield return transform.DOScale(0.05f, 0.4f).From().SetEase(Ease.OutCubic).WaitForCompletion();
		yield return new WaitForSeconds(0.1f);
		causeDamage = false; //TODO destroy collider.
		yield return transform.GetComponent<Renderer>().material.DOColor(Color.clear, 0.5f).SetEase(Ease.OutQuad).WaitForCompletion();
		Destroy(gameObject);
		//TODO fade
	}

	void OnTriggerEnter(Collider other) {
		if (!causeDamage)
			return;
		var enemy = other.GetComponentInParent<PingPongEnemy>();
		if (enemy != null) {
			enemy.Destroy();
		}
		var player = other.GetComponentInParent<Player>();
		if (player != null) {
			player.Destroy();
		}
	}
}
