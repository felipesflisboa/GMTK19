using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
	[SerializeField] GameObject explosionPrefab;

	const float DURATION = 1.3f;
	
	IEnumerator Start () {
		yield return new WaitForSeconds(DURATION);
		Instantiate(explosionPrefab, transform.position, transform.rotation);
		yield return new WaitForSeconds(0.1f);
		Destroy(gameObject);
	}
}
