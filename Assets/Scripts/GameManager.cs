using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO HUD
public class GameManager : SingletonMonoBehaviour<GameManager> {
	[SerializeField] GameObject playerPrefab;

	void Awake() {
		Instantiate(playerPrefab, Stage.I.startPoint.position, Stage.I.startPoint.rotation);
		Camera.main.gameObject.AddComponent<CameraController>();
	}

	public void ResetStage() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void LoadNextStage() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
	}
}
