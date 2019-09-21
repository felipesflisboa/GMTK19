using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager> {
	[SerializeField] GameObject playerPrefab;
	[SerializeField] GameObject canvasPrefab;
	[SerializeField] StageData stageData;
	[SerializeField] AudioSource bgm;
	public Stage currentStage { get; private set; }
	public Player player { get; private set; }
	public CanvasController canvasController { get; private set; }
	internal float elapsedTime;
	internal float totalElapsedTime; // Include resets
	bool resetting;

	public const float INITIAL_SETUP_TIME = 0.6f;
	public const float TIME_LIMIT = 60;

	public float RemainingTime => TIME_LIMIT - elapsedTime;
	public float ClampedRemainingTime => Mathf.Max(0, RemainingTime);
	public float RemainingTimeRatio => ClampedRemainingTime / TIME_LIMIT;
	public bool TimeEnded => RemainingTime < 0;
	public bool Paused => Time.timeScale == 0;

	void Awake() {
		if (Active && I != this) {
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
		totalElapsedTime += Time.deltaTime;
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
		if (Paused)
			bgm.Pause();
		else
			bgm.Play();
	}
}
