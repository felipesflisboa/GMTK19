using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO remove singleton
public class Player : SingletonMonoBehaviour<Player> {
	public GameObject missilePrefab;
	public GameObject bombPrefab;

	internal Dictionary<string, bool> commandsUsed = new Dictionary<string, bool>();
	Rigidbody rb;
	bool started; // For won't mess button press
	bool crouched;
	float initialScaleY;

	public const float INITIAL_SETUP_TIME = 0.6f;
	const float CROUCH_SCALE = 0.3f;
	const float SPEED = 10f;

	void Awake () {
		rb = GetComponent<Rigidbody>();
		initialScaleY = transform.localScale.y;
		foreach (var s in new[] {"Fire1","Fire2", "Fire3", "Jump", "Left", "Right", "Up", "Down" })
			commandsUsed.Add(s, false);
		this.Invoke(new WaitForSeconds(INITIAL_SETUP_TIME), () => started = true);
	}
	
	void Update () {
		if (!started)
			return;
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
		if (!crouched && GetButtonDownValid("Up"))
			rb.AddForce(Vector3.up * 10, ForceMode.VelocityChange);

		if (GetButtonDownValid("Down")) {
			crouched = true;
			transform.DOScaleY(CROUCH_SCALE, 0.4f).SetEase(Ease.OutSine);
		}
		if (crouched && Input.GetButtonUp("Down")) {
			crouched = false;
			DOTween.Kill(transform);
			transform.DOScaleY(initialScaleY, 0.6f).SetEase(Ease.InSine);
		}

		if (GetButtonDownValid("Fire1")) {
			rb.AddForce(-transform.forward * 3, ForceMode.VelocityChange);
			Instantiate(missilePrefab, transform.position + transform.forward*0.5f, transform.rotation);
		}

		if (GetButtonDownValid("Fire2")) {
			Instantiate(bombPrefab, transform.position + Vector3.down * 0.3f, Quaternion.identity);
		}

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

	public void Destroy() {
		GameManager.I.RestartStage();
	}
}
