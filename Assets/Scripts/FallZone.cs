using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallZone : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		var player = other.GetComponentInParent<Player>();
		if (player!=null)
			player.Die();
	}
}
