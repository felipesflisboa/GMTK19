using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PingPongEnemy : MonoBehaviour, IDestructible {
	[SerializeField] int axis;
	[SerializeField] float speed;
	[SerializeField] float distance;
	[SerializeField] bool positive = true;
	Rigidbody rigidBody;
	bool destroying;

	Vector3 BaseVelocity {
		get {
			Vector3 ret = Vector3.zero;
			ret[axis] = speed;
			return ret;
		}
	}

	void Awake() {
		rigidBody = GetComponent<Rigidbody>();
		StartCoroutine(MainRoutine());
	}

	IEnumerator MainRoutine() {
		yield return null;
		while (!destroying) {
			rigidBody.velocity = (positive ? 1 : -1) * BaseVelocity;
			yield return new WaitForSeconds(distance / speed);
			positive = !positive;
		}
	}

	void OnTriggerEnter(Collider other) {
		var player = other.GetComponentInParent<Player>();
		if (player != null)
			player.Destroy();
	}

	public void Destroy() {
		if (destroying)
			return;
		destroying = true;
		rigidBody.velocity = Vector3.zero;
		foreach (Collider col in GetComponentsInChildren<Collider>())
			col.enabled = false;
		transform.DOScale(transform.localScale/30f, 0.25f).SetEase(Ease.InOutSine).OnComplete(() => Destroy(gameObject));
	}
}
