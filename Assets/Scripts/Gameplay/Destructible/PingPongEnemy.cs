using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongEnemy : MonoBehaviour, IDestructible {
	[SerializeField] GameObject explosionPrefab;
	[SerializeField] int axis;
	[SerializeField] float speed;
	[SerializeField] float distance;
	[SerializeField] bool positive = true;
	Rigidbody rigidBody;

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
		while (true) {
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
		Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
