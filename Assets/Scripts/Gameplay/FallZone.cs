using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallZone : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		Player player = other.GetComponentInParent<Player>();
		if (player!=null)
			player.Destroy();
	}
}
