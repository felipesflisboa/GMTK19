using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO remove singleton
public class Player : SingletonMonoBehaviour<Player> {
	public GameObject missilePrefab;
	public GameObject bombPrefab;
	public Transform missileShootPoint;
	public Transform bombShootPoint;
	public Collider mainCollider;
	public Collider crouchingCollider;

	internal Dictionary<string, bool> commandsUsed = new Dictionary<string, bool>();
	bool started; // For won't mess button press
	bool resetting; // For won't mess button press
	bool crouching;
	//float initialMainColliderScaleY; //TODO rename
	//float initialMainColliderPosY;

	Rigidbody rb;
	Animator animator;

	public const float INITIAL_SETUP_TIME = 0.6f;
	const float SPEED = 10f;
	const string ANIMATOR_CROUCH_KEY = "Crouching";

	void Awake () {
		rb = GetComponent<Rigidbody>();
		animator = GetComponentInChildren<Animator>();
		// initialMainColliderScaleY = mainCollider.transform.localScale.y;
		// initialMainColliderPosY = mainCollider.transform.localPosition.y;
		foreach (var s in new[] {"Fire1","Fire2", "Fire3", "Jump", "Submit", "Left", "Right", "Up", "Down" })
			commandsUsed.Add(s, false);
		this.Invoke(new WaitForSeconds(INITIAL_SETUP_TIME), () => started = true);
		mainCollider.gameObject.SetActive(true); //TODO redo
		crouchingCollider.gameObject.SetActive(false);
	}
	
	void Update () {			
		if (!started)
			return;

		if (GetButtonDownValid("Submit")) {
			Time.timeScale = Time.timeScale == 0 ? 1 : 0;
		}

		if (Time.timeScale == 0)
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
		if (!crouching && GetButtonDownValid("Up"))
			rb.AddForce(Vector3.up * 10, ForceMode.VelocityChange);

		if (GetButtonDownValid("Down")) {
			//TODO redo in method
			crouching = true;
			animator.SetBool(ANIMATOR_CROUCH_KEY, true);
			mainCollider.gameObject.SetActive(false);
			crouchingCollider.gameObject.SetActive(true);
			//TODO redo
			//mainCollider.transform.DOScaleY(CROUCH_SCALE, 0.4f).SetEase(Ease.OutSine);
			//mainCollider.transform.DOLocalMoveY(initialMainColliderPosY - (initialMainColliderScaleY - CROUCH_SCALE)*2/3f, 0.4f).SetEase(Ease.OutSine);
		}
		if (crouching && Input.GetButtonUp("Down")) {
			crouching = false;
			animator.SetBool(ANIMATOR_CROUCH_KEY, false);
			mainCollider.gameObject.SetActive(true);
			crouchingCollider.gameObject.SetActive(false);
			//DOTween.Kill(mainCollider.transform);
			//TODO redo
			//mainCollider.transform.DOScaleY(initialMainColliderScaleY, 0.6f).SetEase(Ease.InSine);
			//mainCollider.transform.DOLocalMoveY(initialMainColliderPosY, 0.6f).SetEase(Ease.InSine);
		}

		if (GetButtonDownValid("Fire1")) {
			rb.AddForce(-transform.forward * 3, ForceMode.VelocityChange);
			Instantiate(missilePrefab, missileShootPoint.position, missileShootPoint.rotation);
		}

		if (GetButtonDownValid("Fire2")) {
			Instantiate(bombPrefab, bombShootPoint.position, Quaternion.identity);
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
