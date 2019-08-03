using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO remove singleton
public class Player : SingletonMonoBehaviour<Player> {
	internal Dictionary<string, bool> commandsUsed = new Dictionary<string, bool>();
	Rigidbody rb;

	const float SPEED = 5f;

	void Awake () {
		rb = GetComponent<Rigidbody>();
		foreach(var s in new[] {"Fire1","Fire2", "Left", "Right", "Up", "Down" })
			commandsUsed.Add(s, false);
	}
	
	void Update () {
		if (GetButtonValid("Left") ^ GetButtonValid("Right")) {
			float newHorizonInput = 0f;
			if (GetButtonValid("Left")) {
				newHorizonInput = -Input.GetAxis("Left");
				rb.rotation = Quaternion.Euler(Vector3.up * 270);
			}
			if (GetButtonValid("Right")) {
				newHorizonInput = Input.GetAxis("Right");
				rb.rotation = Quaternion.Euler(Vector3.up * 90);
			}
			rb.MovePosition(rb.position + newHorizonInput * Time.deltaTime * SPEED * Vector3.right); //TODO redo
		}
		if (GetButtonDownValid("Up"))
			rb.AddForce(Vector3.up * 10, ForceMode.VelocityChange);

		if (Input.GetButton("Jump"))
			GameManager.I.ResetStage();

		//TODO look alloc
		foreach (var k in new List<string>(commandsUsed.Keys))
			if (Input.GetButtonUp(k))
				commandsUsed[k] = true;
	}

	bool GetButtonValid(string p) {
		return !commandsUsed[p] && Input.GetButton(p);
	}

	bool GetButtonDownValid(string p) {
		return !commandsUsed[p] && Input.GetButtonDown(p);
	}

	public void Die() {
		GameManager.I.ResetStage();
	}
}
