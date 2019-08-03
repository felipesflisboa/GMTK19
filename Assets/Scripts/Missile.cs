using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Missile : MonoBehaviour {
	Rigidbody rb;

	const float MAX_SPEED = 30f;

	void Start () {
		rb = GetComponent<Rigidbody>();
		DOTween.To((val) => rb.velocity = transform.forward * val, 0f, MAX_SPEED, 4f).SetEase(Ease.OutQuad); // .SetTarget(this);
	}

	void OnTriggerEnter(Collider other) {
		var enemy = other.GetComponentInParent<PingPongEnemy>();
		if (enemy != null) {
			enemy.Destroy();
			Destroy();
		}
	}

	public void Destroy() {
		Destroy(gameObject);
	}
}
