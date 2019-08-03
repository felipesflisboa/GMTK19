using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongEnemy : MonoBehaviour {
	[SerializeField] int axis;
	[SerializeField] float speed;
	[SerializeField] float distance;
	[SerializeField] bool positive = true;

	IEnumerator Start() {
		//TODO redo
		Vector3 velocity = Vector3.zero;
		velocity[axis] = speed;
		var rb = GetComponent<Rigidbody>();
		while (true) {
			rb.velocity = (positive ? 1 : -1)*velocity;
			yield return new WaitForSeconds(distance/speed);
			positive = !positive;
		}
	}

	void OnTriggerEnter(Collider other) {
		var player = other.GetComponentInParent<Player>();
		if (player != null)
			player.Destroy();
	}

	public void Destroy() {
		Destroy(gameObject);
	}
}
