using UnityEngine;
public class Game
{
	public enum GameStatus
    {
		isInMenu,
		isGameover,
		isPlaying,
		isPaused,
		isGameWin
	}
	public static GameStatus gameStatus;
	public static bool isGameover = false;
	public static bool isMoving = false;
	public static bool playerIdDead;
    public static string itemTag = "Object";
    public static string powerupTag = "Powerup";
	public static string blastTag = "Obstacle";
	public static string diemondTag = "Diemond";
	public static string coinTag = "Coin";
	public static string shop = "Shop";

	public static string levelKey = "levelKey";
	public static string Menu = "Menu";
	public static string Level = "Level";
	public static string HUD = "HUD";
	public static string Gameover = "Gameover";
	public static string GameWin = "GameWin";
	public static string Pause = "Pause";
	public static string enemyTag = "Enemy";
	public static string playerTag = "Player";	

	public static int coinToGive=10;
	public static int diemondToGive = 3;
	public static int lifeToGive = 1;
	public static int currentScore;

	public static int pistolBulletPrice=100;
	public static int akBulletPrice = 300;
	public static int rifleBulletPrice = 300;
	public static int lifePrice = 500;


	public static int retryCount
	{
		get { return PlayerPrefs.GetInt("retryCount", 0); }
		set { PlayerPrefs.SetInt("retryCount", value); }
	}
	public static int TotalCoins
	{
		get { return PlayerPrefs.GetInt("TotalCoins", 0); }
		set { PlayerPrefs.SetInt("TotalCoins", value); }
	}
	public static int TotalDiemonds
	{
		get { return PlayerPrefs.GetInt("TotalDiemonds", 0); }
		set { PlayerPrefs.SetInt("TotalDiemonds", value); }
	}
	public static int CurrentLevel
	{
		get { return PlayerPrefs.GetInt("CurrentLevel", 0); }
		set { PlayerPrefs.SetInt("CurrentLevel", value); }
	}
	public static int HighScore
	{
		get { return PlayerPrefs.GetInt("HighScore", 0); }
		set { PlayerPrefs.SetInt("HighScore", value); }
	}
    public static int Life
    {
        get { return PlayerPrefs.GetInt("Life", 0); }
        set { PlayerPrefs.SetInt("Life", value); }
    }
	public static int PistolBullet
	{
		get { return PlayerPrefs.GetInt("PistolBullet", 0); }
		set { PlayerPrefs.SetInt("PistolBullet", value); }
	}
	public static int AKBullet
	{
		get { return PlayerPrefs.GetInt("AKBullet", 0); }
		set { PlayerPrefs.SetInt("AKBullet", value); }
	}
	public static int RifleBullet
	{
		get { return PlayerPrefs.GetInt("RifleBullet", 0); }
		set { PlayerPrefs.SetInt("RifleBullet", value); }
	}
}
