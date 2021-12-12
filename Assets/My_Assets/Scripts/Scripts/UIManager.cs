using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using suja;

public class UIManager : MonoBehaviour
{
	#region Singleton class: UIManager

	public static UIManager Instance;

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		}
		//OffAllUIObjects();		
		StartCoroutine( ActiveBack(true,0));
		Game.currentScore = 0;
	}

	#endregion

	[Header ("Level Progress UI")]
	//sceneOffset: because you may add other scenes like (Main menu, ...)
	//[SerializeField] int sceneOffset;
	[SerializeField] Text nextLevelText;
	[SerializeField] Text currentLevelText;
	//[SerializeField] Image progressFillImage;

	[Space]
	[SerializeField] Text levelCompletedText;

	[Space]
	//white fading panel at the start
	[SerializeField] Image fadePanel;

	[SerializeField] List<GameObject> UIObjects;	
	[SerializeField] GameObject back;
	[SerializeField] Button pauseButton;
	[SerializeField] GameObject transition;
	Jumping_Ball player;
	public IEnumerator ActiveBack(bool value,float wait)
    {
		yield return new WaitForSeconds(wait);
		//back.SetActive(value);
    }
	void Start ()
	{
		//reset progress value
		//progressFillImage.fillAmount = 0f;	
		//if (Game.retryCount == 0)
  //      {
		ShowUI(Game.Menu);
	    StartCoroutine(MusicManager.PlayMusic("menu",2));
		//}
		pauseButton.onClick.AddListener(OnPause);
		player = FindObjectOfType<Jumping_Ball>();
	}
	void OffAllUIObjects()
	{
		for (int i = 0; i < UIObjects.Count; i++)
		{
			UIObjects[i].SetActive(false);
		}
	}
	void PlayButtonClip()
    {
		MusicManager.PlaySfx("button");		
    }

	public GameObject UIObject(string name)
	{
		int objectIndex = UIObjects.FindIndex(gameObject => string.Equals(name, gameObject.name));
		return UIObjects[objectIndex];
	}
	public void WaitAndShowUI(float wait, string uiName)
	{
		waitTime = wait;
		StartCoroutine(ContineuShowUI(uiName));
	}
	float waitTime;
	public void ShowUI(string uiName)
	{
		StartCoroutine(ContineuShowUI(uiName));
	}
	IEnumerator PlayTransition()
	{
		//SoundManager.PlaySfx("transition");
		transition.SetActive(false);
		yield return new WaitForSeconds(0.1f);
		transition.SetActive(true);
		yield return new WaitForSeconds(2f);
		transition.SetActive(false);

	}
	IEnumerator ContineuShowUI(string uiName)
	{
		yield return new WaitForSeconds(waitTime);
		//SoundManager.PlaySfx("button");
		StartCoroutine(PlayTransition());
		yield return new WaitForSeconds(1f);
		OffAllUIObjects();
		UIObject(uiName).SetActive(true);
		waitTime = 0;
		Button[] allButtons = FindObjectsOfType<Button>();
        if (allButtons.Length > 0)
        {
            for (int i = 0; i < allButtons.Length; i++)
            {
				allButtons[i].onClick.AddListener(PlayButtonClip);
			}
        }
		//AdmobAdmanager.Instance.ShowInterstitial();
	}

	//--------------------------------------
	public void ShowLevelCompletedUI ()
	{
		//fade in the text (from 0 to 1) with 0.6 seconds
		levelCompletedText.DOFade (1f, .6f).From (0f);
	}

	public void Fade ()
	{
		//fade out the panel (from 1 to 0) with 1.3 seconds
		fadePanel.DOFade (0f, 1.3f).From (1f);
	}	
	public void OnGameover()
    {
		Game.gameStatus = Game.GameStatus.isGameover;
		ShowUI(Game.Gameover);
		StartCoroutine(Gameover());
		MusicManager.PauseMusic(0.2f); 
	}
	IEnumerator Gameover()
    {		
		yield return new WaitForSeconds(1.2f);
		//StartCoroutine(ActiveBack(true, 0));
		//Game.retryCount=0;
		Text High_Score_num = UIObject(Game.Gameover).transform.Find("High_Score_num").GetComponent<Text>();
		Text header = UIObject(Game.Gameover).transform.Find("header").GetComponent<Text>();
	    Text coin_num = UIObject(Game.Gameover).transform.Find("coin_num").GetComponent<Text>();
		Text diemond_num = UIObject(Game.Gameover).transform.Find("diemond_num").GetComponent<Text>();
		Text current_Score_num = UIObject(Game.Gameover).transform.Find("current_Score_num").GetComponent<Text>();
		High_Score_num.text = Game.HighScore.ToString();
		coin_num.text = Game.TotalCoins.ToString();
		diemond_num.text = Game.TotalDiemonds.ToString();
		current_Score_num.text = Game.currentScore.ToString();
		Button retryButton=UIObject(Game.Gameover).transform.Find("Retry").GetComponent<Button>();
		retryButton.onClick.AddListener(RetryLevel);
		Button homeButton = UIObject(Game.Gameover).transform.Find("home").GetComponent<Button>();
		homeButton.onClick.AddListener(GoHome);
        if (Game.currentScore > Game.HighScore)
        {
			header.text = "New Score";
			Game.HighScore = Game.currentScore;
			MusicManager.PlaySfx("new_score");
		}
        else
        {
			header.text = "Gameover";
			MusicManager.PlaySfx("gameover");
		}
	}
	
	public void OnLevelWon()
	{
		StartCoroutine(ActiveBack(true, 1.5f));
	    WaitAndShowUI(2.0f, Game.GameWin);
		StartCoroutine(LevelWon());			
	}
	IEnumerator LevelWon()
	{
		yield return new WaitForSeconds(.2f);		
		//Game.retryCount = 0;
		Button retryButton = UIObject(Game.GameWin).transform.Find("Retry").GetComponent<Button>();
		retryButton.onClick.AddListener(RetryLevel);
		Button homeButton = UIObject(Game.GameWin).transform.Find("home").GetComponent<Button>();
		homeButton.onClick.AddListener(GoHome);		
	}
	void GoHome()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	public void RetryLevel()
    {
		//Game.retryCount++;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);		
    }
	IEnumerator Retry()
    {
	
		yield return new WaitForSeconds(2);
        //SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
  //      if (Game.retryCount > 0)
  //      {
		//	//levelHanler.ActivateLevel(Game.CurrentLevel);
		//}
		
	}
	void OnResume()
    {
		Game.gameStatus = Game.GameStatus.isPlaying;
		ShowUI(Game.HUD);
		MusicManager.PlayMusic("Gameloop-16");
		//StartCoroutine(ActiveBack(false, 1));
	}
	void OnPause()
    {
		Game.gameStatus = Game.GameStatus.isPaused;
		ShowUI(Game.Pause);
		MusicManager.PauseMusic(0.2f);
		//StartCoroutine(ActiveBack(true, 0.7f));
		StartCoroutine(Pause());
	}
	IEnumerator Pause()
    {
		yield return new WaitForSeconds(1.2F);		
		Button resumeButton = UIObject(Game.Pause).transform.Find("RESUME").GetComponent<Button>();
		resumeButton.onClick.AddListener(OnResume);
	}
	void OnEnable()
	{		
		//SceneManager_New.onSceneLoaded += OnSceneLoaded;
	}

	// called second
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		StartCoroutine(Retry());
		//Debug.Log("OnSceneLoaded: " + scene.name);
		//Debug.Log(mode);
		
	}
	
	// called when the game is terminated
	void OnDisable()
	{		
		//SceneManager_New.onSceneLoaded -= OnSceneLoaded;
	}

}
