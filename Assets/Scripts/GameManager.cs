using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO HUD
public class GameManager : SingletonMonoBehaviour<GameManager> {
	public GameObject playerPrefab;
	[SerializeField] GameObject canvasPrefab;
	internal float timeElapsed;

	const float TIME_LIMIT = 60;

	//TODO reverse name
	public float TimeRemaining {
		get {
			return TIME_LIMIT - timeElapsed;
		}
	}

	public float TimeRemainingClamped {
		get {
			return Mathf.Max(0, TimeRemaining);
		}
	}

	public float TimeRemainingRatio {
		get {
			return TimeRemainingClamped / TIME_LIMIT;
		}
	}

	public bool TimeEnded {
		get {
			return TimeRemaining < 0;
		}
	}

	void Awake() {
		if (FindObjectsOfType<GameManager>().Length > 1) {
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		Instantiate(canvasPrefab);
	}

	public void ResetStage() {
		timeElapsed = Stage.I.startTime;
		RestartStage();
	}

	public void RestartStage() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void LoadNextStage() {
		if(SceneManager.GetActiveScene().name == "StageH") {
			int lastScore = Mathf.RoundToInt(TimeRemaining * 1000);
			var slt = new ScoreListTimed(); //TODO redo
			slt.AddScore(lastScore);
			slt.Save();
			ScoreListTimedDrawer.lastScore = lastScore;
			LoadInitialScreen();
			return;
		}
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
	}

	public void LoadInitialScreen() {
		SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
		SceneManager.MoveGameObjectToScene(FindObjectOfType<CanvasController>().gameObject, SceneManager.GetActiveScene());
		SceneManager.LoadScene("MainMenu");
	}

	void Update() {
		timeElapsed += Time.deltaTime;
		if (TimeEnded)
			LoadInitialScreen();
	}
}
