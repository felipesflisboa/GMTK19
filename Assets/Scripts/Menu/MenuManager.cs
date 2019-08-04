using UnityEngine;
using System.Collections;

/// <summary>
/// Sample option class.
/// Version 1.1
/// </summary>
public class MenuManager : SingletonMonoBehaviour<MenuManager> {
	public enum Option{
		None=0,Title,Info,HighScores
	}
	[SerializeField] AudioClip seClick;
	Option currentPanelOption;
	MenuPanel[] panelArray;
	Timer clickCooldownTimer;

	void Start () {
		clickCooldownTimer = new Timer(0.75f);

		panelArray = GetComponentsInChildren<MenuPanel>(true);

		bool hasScore = ScoreListTimedDrawer.lastScore!=null;
		Option newPanelOption = hasScore ? Option.HighScores : Option.Title;
		EnablePanel(newPanelOption);
	}

	public void EnablePanel(Option panelOption){
		currentPanelOption = panelOption;
		foreach(MenuPanel panel in panelArray){
			panel.gameObject.SetActive(panel.panelType==panelOption);
		}
	}

	void PlayClickSE(){
		if(seClick!=null)
			AudioUtil.PlaySE(seClick);
	}

#region Buttons
	public void Play(){
		PlayClickSE();
		ScoreListTimedDrawer.lastScore = null;
		UnityEngine.SceneManagement.SceneManager.LoadScene("StageA");
	}

	public void Info(){
		PlayClickSE();
		EnablePanel(Option.Info);
	}

	public void HighScores(){
		PlayClickSE();
		EnablePanel(Option.HighScores);
	}

	public void Exit(){
		PlayClickSE();
		Application.Quit();
	}
#endregion

	void Update(){
		bool backToTitle = Input.GetButtonDown("Fire1") && currentPanelOption!=Option.Title && clickCooldownTimer.CheckAndUpdate();
		if(backToTitle){
			PlayClickSE();
			EnablePanel(Option.Title);
		}
	}
}
