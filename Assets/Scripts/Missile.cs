using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Missile : MonoBehaviour {
	Rigidbody rb;

	const float MAX_SPEED = 15f;

	void Start () {
		rb = GetComponent<Rigidbody>();
		DOTween.To((val) => rb.velocity = transform.forward * val, 0f, MAX_SPEED, 0.5f).SetEase(Ease.OutQuad); // .SetTarget(this);
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
