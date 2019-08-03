using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO remove singleton
public class Player : SingletonMonoBehaviour<Player> {
	internal Dictionary<string, bool> commandsUsed = new Dictionary<string, bool>();
	/*
	internal Dictionary<string, bool> positiveAxisUsed = new Dictionary<string, bool>();
	internal Dictionary<string, bool> negativeAxisUsed = new Dictionary<string, bool>();
	*/
	Rigidbody rb;

	const float SPEED = 5f;

	void Awake () {
		rb = GetComponent<Rigidbody>();
		foreach(var s in new[] {"Fire1","Fire2", "Left", "Right", "Up", "Down" })
			commandsUsed.Add(s, false);
		/*
		foreach (var s in new[] { "Horizontal"}) {
			positiveAxisUsed.Add(s, false);
			negativeAxisUsed.Add(s, false);
		}
		*/
	}
	
	void Update () {
		/*
		if()
		rigidbody.ad
		*/
		//TODO redo
		float newHorizonInput = 0f;
		if (GetButtonValid("Left"))
			newHorizonInput -= Input.GetAxis("Left");
		if (GetButtonValid("Right"))
			newHorizonInput += Input.GetAxis("Right");
		rb.position+= newHorizonInput * Time.deltaTime * SPEED * Vector3.right;
		if (GetButtonDownValid("Up"))
			rb.AddForce(Vector3.up*10, ForceMode.VelocityChange);

		if(Input.GetButton("Jump"))
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

		//TODO look alloc
		foreach (var k in new List<string>(commandsUsed.Keys))
			if (Input.GetButtonUp(k))
				commandsUsed[k] = true;
		/*
		foreach (var k in new List<string>(positiveAxisUsed.Keys))
			if (Input.GetAxisRaw(k) ==1)
				positiveAxisUsed[k] = true;
		foreach (var k in new List<string>(negativeAxisUsed.Keys))
			if (Input.GetAxisRaw(k) == -1)
				negativeAxisUsed[k] = true;
		*/
	}

	bool GetButtonValid(string p) {
		return !commandsUsed[p] && Input.GetButton(p);
	}

	bool GetButtonDownValid(string p) {
		return !commandsUsed[p] && Input.GetButtonDown(p);
	}

	/*
	//TODO merge code
	float GetAxisRawValid(string p) {
		return (!positiveAxisUsed[p] && Input.GetAxisRaw(p) == 1) || (!negativeAxisUsed[p] && Input.GetAxisRaw(p) == -1) ? Input.GetAxisRaw(p) : 0f;
	}

	float GetAxisValid(string p) {
		return (!positiveAxisUsed[p] && Input.GetAxisRaw(p) == 1) || (!negativeAxisUsed[p] && Input.GetAxisRaw(p) == -1) ? Input.GetAxis(p) : 0f;
	}
	*/
}
