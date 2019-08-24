using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDestructible {
	public GameObject missilePrefab;
	public GameObject bombPrefab;
	public GameObject sparkPrefab;
	public Transform missileShootPoint;
	public Transform bombShootPoint;
	public Collider mainCollider;
	public Collider crouchingCollider;
	Rigidbody rigidBody;
	Animator animator;
	public CommandHandler commandHandler = new CommandHandler();
	bool started; // For won't mess button press

	const float SPEED = 10f;
	const string ANIMATOR_CROUCH_KEY = "Crouching";
	const string ANIMATOR_RUNNING_KEY = "Running";

	float ValidHorizontalInput {
		get {
			return commandHandler.GetButtonValid("Right") ? Input.GetAxis("Right") : -Input.GetAxis("Left");
		}
	}

	bool _crouching;
	bool Crouching {
		get {
			return _crouching;
		}
		set {
			_crouching = value;
			animator.SetBool(ANIMATOR_CROUCH_KEY, value);
			mainCollider.gameObject.SetActive(!value);
			crouchingCollider.gameObject.SetActive(value);
		}
	}

	void Awake () {
		rigidBody = GetComponent<Rigidbody>();
		animator = GetComponentInChildren<Animator>();
		commandHandler.Setup(new[] { "Fire1", "Fire2", "Jump", "Submit", "Left", "Right", "Up", "Down" });
		this.Invoke(new WaitForSeconds(GameManager.INITIAL_SETUP_TIME), () => started = true);
		mainCollider.gameObject.SetActive(true);
		crouchingCollider.gameObject.SetActive(false);
	}
	
	void Update () {			
		if (!started)
			return;
		UpdatePauseButtonAction();
		if (!GameManager.I.Paused) {
			UpdateButtonsActions();
			UpdateCommandsUsed();
		}
	}

	/// <summary>
	/// Updates pause button action.
	/// 
	/// This one is updated before the other ones, since it validation may stop other buttons.
	/// </summary>
	void UpdatePauseButtonAction() {
		if (commandHandler.GetButtonDownValid("Submit"))
			GameManager.I.TogglePause();
	}

	void UpdateButtonsActions() {
		if (commandHandler.GetButtonValid("Left") ^ commandHandler.GetButtonValid("Right"))
			StartMoving();
		else
			StopMoving();
		if (!Crouching && commandHandler.GetButtonDownValid("Up"))
			Jump();
		if (commandHandler.GetButtonDownValid("Down"))
			Crouching = true;
		if (Crouching && Input.GetButtonUp("Down"))
			Crouching = false;
		if (commandHandler.GetButtonDownValid("Fire1"))
			FireMissile();
		if (commandHandler.GetButtonDownValid("Fire2"))
			FireBomb();
		if (Input.GetButton("Jump"))
			GameManager.I.ResetStage();
	}

	void StartMoving() {
		rigidBody.rotation = Quaternion.Euler(Vector3.up * (commandHandler.GetButtonValid("Left") ? 270 : 90));
		rigidBody.MovePosition(rigidBody.position + ValidHorizontalInput * Time.deltaTime * SPEED * Vector3.right);
		animator.SetBool(ANIMATOR_RUNNING_KEY, true);
	}

	void StopMoving() {
		animator.SetBool(ANIMATOR_RUNNING_KEY, false);
	}

	void Jump() {
		rigidBody.AddForce(Vector3.up * 10, ForceMode.VelocityChange);
	}

	void FireMissile() {
		rigidBody.AddForce(-transform.forward * 3, ForceMode.VelocityChange);
		Instantiate(missilePrefab, missileShootPoint.position, missileShootPoint.rotation);
	}

	void FireBomb() {
		Instantiate(bombPrefab, bombShootPoint.position, Quaternion.identity);
	}
	
	void UpdateCommandsUsed() {
		if (commandHandler.NewCommandsUsedThisFrame == 0)
			return;
		CreateSparks(commandHandler.NewCommandsUsedThisFrame);
		commandHandler.RefreshCommandsUsed();
	}

	void CreateSparks(int count) {
		for (int i = 0; i < count; i++)
			Destroy(Instantiate(sparkPrefab, transform.position, Quaternion.identity), 5f);
	}

	public void Destroy() {
		GameManager.I.RestartStage();
	}
}
