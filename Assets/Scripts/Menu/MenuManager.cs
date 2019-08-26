using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// Sample option class.
/// Version 1.1
/// </summary>
public class MenuManager : SingletonMonoBehaviour<MenuManager> {
	public enum Option{
		None=0,Title,Info,HighScores,Loading,GameOver
	}

	public static Option nextInitialScreen = Option.Title;

	[SerializeField] StageData stageData;
	[SerializeField] AudioSource clickSFX;
	Option currentPanelOption;
	MenuPanel[] panelArray;

	string Version { //remove
		get {
			return "1.1";
		}
	}

	void Awake() {
		panelArray = GetComponentsInChildren<MenuPanel>(true);
	}

	void Start () {
		EnablePanel(nextInitialScreen);
		//versionLabel.text = string.Format("V {0}", Application.version).Replace(".",";"); //remove
		nextInitialScreen = Option.Title;
	}

	void OnEnable() {
		StartCoroutine(ClickRoutine());
	}

	public void EnablePanel(Option panelOption){
		currentPanelOption = panelOption;
		foreach(MenuPanel panel in panelArray)
			panel.gameObject.SetActive(panel.panelType==panelOption);
	}

	void PlayClickSE(){
		if(clickSFX != null)
			clickSFX.Play();
	}

#region Buttons
	public void OnPlayClick(){
		PlayClickSE();
		ScoreListTimedDrawer.lastScore = null;
		EnablePanel(Option.Loading);
		this.Invoke(new WaitForEndOfFrame(), ()=> UnityEngine.SceneManagement.SceneManager.LoadScene(stageData.array[0].ScenePath));
	}

	public void OnInfoClick(){
		PlayClickSE();
		EnablePanel(Option.Info);
	}

	public void OnHighScoreClick(){
		PlayClickSE();
		EnablePanel(Option.HighScores);
	}
#endregion

	IEnumerator ClickRoutine(){
		while (true) {
			yield return new WaitForSeconds(0.75f);
			yield return new WaitUntil(() => Input.GetButtonDown("Fire1") && !(new[] { Option.Title, Option.Loading }).Contains(currentPanelOption));
			PlayClickSE();
			EnablePanel(Option.Title);
		}
	}
}
