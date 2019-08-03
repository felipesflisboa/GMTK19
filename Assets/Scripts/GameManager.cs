using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO HUD
public class GameManager : SingletonMonoBehaviour<GameManager> {
	[SerializeField] GameObject playerPrefab;

	void Awake () {
		Instantiate(playerPrefab, Stage.I.startPoint.position, Stage.I.startPoint.rotation);
		Camera.main.gameObject.AddComponent<CameraController>();
	}

	public void ResetStage() {
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
	}
}
