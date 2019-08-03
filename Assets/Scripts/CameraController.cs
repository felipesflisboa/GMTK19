using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class CameraController : MonoBehaviour {
	
	void Update () {
		transform.position = transform.position.WithX(Mathf.Lerp(transform.position.x, Player.I.transform.position.x, 5*Time.deltaTime));
	}
}
