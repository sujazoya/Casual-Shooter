using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class MassegeItems
{
	public GameObject massegeCanvas;
	public Text header;
	public Text massege;
	public Button closeButton;

	public GameObject popupCanvas;
	public Text popupText;
}


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
	[SerializeField]MassegeItems massegeItems;

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
	[SerializeField] GameObject settingPanel;
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
		massegeItems.closeButton.onClick.AddListener(CloseMassegeCanvas);
		CloseMassegeCanvas();
	}
	public void ShowPopup(string massege)
    {
		StartCoroutine(ShowPopup_(massege));
    }
	IEnumerator ShowPopup_(string massege)
    {
		massegeItems.popupText.text = string.Empty;
		massegeItems.popupCanvas.SetActive(true);
		massegeItems.popupText.text = massege;
		yield return new WaitForSeconds(2.5f);
		massegeItems.popupText.text = string.Empty;
		massegeItems.popupCanvas.SetActive(false);
	}
	void CloseMassegeCanvas()
    {
		massegeItems.massegeCanvas.SetActive(false);
	}
	public void ShowMassege(string header,string massege)
    {
		massegeItems.massegeCanvas.SetActive(true);
		massegeItems.header.text = header;
		massegeItems.massege.text = massege;
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
	public void ShowShop()
    {
		ShowUI(Game.shop);
    }
	public void CloseShop()
	{
		ShowUI(Game.Menu);
	}
	public void PurchaseAkMag()
    {
        if (Game.TotalCoins >= Game.akBulletPrice)
        {
			Game.TotalCoins -= Game.akBulletPrice;
			Game.AKBullet += 27;
			ShowMassege
				("hurray"
				, "You Purchased AK 47 Mag");
        }
        else
        {
			ShowMassege
				("Oh No!"
				, "You Don't Have Enough Credits");
		}
    }
	public void PurchaseRifleMag()
	{
		if (Game.TotalCoins >= Game.rifleBulletPrice)
		{
			Game.TotalCoins -= Game.rifleBulletPrice;
			Game.RifleBullet += 23;
			ShowMassege
				("hurray"
				, "You Purchased Rifle Mag");
		}
		else
		{
			ShowMassege
				("Oh No!"
				, "You Don't Have Enough Credits");
		}
	}
	public void PurchasePistolMag()
	{
		if (Game.TotalCoins >= Game.pistolBulletPrice)
		{
			Game.TotalCoins -= Game.pistolBulletPrice;
			Game.pistolBulletPrice += 9;
			ShowMassege
				("hurray"
				, "You Purchased Pistol Mag");
		}
		else
		{
			ShowMassege
				("Oh No!"
				, "You Don't Have Enough Credits");
		}
	}
	public void PurchaseLife()
	{
		if (Game.TotalCoins >= Game.lifePrice)
		{
			Game.TotalCoins -= Game.lifePrice;
			Game.Life += 1;
			ShowMassege
				("hurray"
				, "You Purchased 1 Life");
		}
		else
		{
			ShowMassege
				("Oh No!"
				, "You Don't Have Enough Credits");
		}
	}
	public void RewardTheUser()
    {
		Game.TotalCoins += 100;
		ShowMassege
				("Congratulation"
				, "You Won 100 Credits");
	}

	public void WarnAdClosed()
	{
		ShowMassege
				("Sorry"
				, "You Closed The AD So You Will Not Get Any Credits");
	}
	
}
