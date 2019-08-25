using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager> {
	[SerializeField] GameObject playerPrefab;
	[SerializeField] GameObject canvasPrefab;
	[SerializeField] StageData stageData;
	public Stage currentStage { get; private set; }
	public Player player { get; private set; }
	public CanvasController canvasController { get; private set; }
	internal float elapsedTime;
	bool resetting;

	public const float INITIAL_SETUP_TIME = 0.6f;
	public const float TIME_LIMIT = 60;

	public float RemainingTime {
		get {
			return TIME_LIMIT - elapsedTime;
		}
	}

	public float ClampedRemainingTime {
		get {
			return Mathf.Max(0, RemainingTime);
		}
	}

	public float RemainingTimeRatio {
		get {
			return ClampedRemainingTime / TIME_LIMIT;
		}
	}

	public bool TimeEnded {
		get {
			return RemainingTime < 0;
		}
	}

	public bool Paused {
		get {
			return Time.timeScale == 0;
		}
	}

	void Awake() {
		if (FindObjectsOfType<GameManager>().Length > 1) {
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		canvasController = Instantiate(canvasPrefab).GetComponent<CanvasController>();
	}

	public void InitializeStage(Stage stage) {
		currentStage = stage;
		player = Instantiate(playerPrefab, currentStage.startPoint.position, Quaternion.Euler(Vector3.up * 90)).GetComponent<Player>();
		Camera.main.gameObject.AddComponent<CameraController>();
	}

	public void ResetStage() {
		elapsedTime = currentStage.startTime;
		RestartStage();
	}

	public void RestartStage() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void LoadNextStage() {
		if(stageData.IsLast(SceneManager.GetActiveScene().path)) {
			RefreshScoreList(Mathf.RoundToInt(RemainingTime * 1000));
			LoadInitialScreen();
			return;
		}
		canvasController.ClearTips();
		SceneManager.LoadScene(stageData.GetNext(SceneManager.GetActiveScene().path).ScenePath);
	}

	void RefreshScoreList(int newMSTime) {
		ScoreListTimed scoreList = new ScoreListTimed();
		scoreList.Load();
		scoreList.AddScore(newMSTime);
		scoreList.Save();
		ScoreListTimedDrawer.lastScore = newMSTime;
		MenuManager.nextInitialScreen = MenuManager.Option.HighScores;
	}

	public void LoadInitialScreen() {
		SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
		SceneManager.MoveGameObjectToScene(FindObjectOfType<CanvasController>().gameObject, SceneManager.GetActiveScene());
		SceneManager.LoadScene("MainMenu");
	}

	void Update() {
		elapsedTime += Time.deltaTime;
		if (TimeEnded) 
			DoGameOver();
	}

	void DoGameOver() {
		MenuManager.nextInitialScreen = MenuManager.Option.GameOver;
		LoadInitialScreen();
	}

	public void TogglePause() {
		Time.timeScale = Paused ? 1 : 0;
	}
}
