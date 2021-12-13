using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MyItems
{
    public GameObject pickupButton;
    public GameObject dropButton;
    public GameObject[] powerup_Texts;    
    public GameObject menu;
    public Button menu_Button;
    public Sprite[] menu_sprites;
    public GameObject aimButton;
    public GameObject bulletTrail;
    public GameObject killEffect;
    public Text coinText;
    public Text diemondText;
    public Text scoreText;
    public Text lifeText;
    public GameObject extraHealthSlider;
    public Image extraHealthfill;
    public GameObject StartPlay;
    public Text number;
}

public class GameController_Grappling : MonoBehaviour
{
    public MyItems items;
    public static GameController_Grappling Instace;
    public static bool isWeaponAimed;
    public GameObject currentBuilding;
    int pwTIndex;
    [HideInInspector] public List<GameObject> bulletTrails;
    UIManager uIManager;
    public GameObject player;  
    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
        }
        for (int i = 0; i < items. powerup_Texts.Length; i++)
        {
            items.powerup_Texts[i].SetActive(false);
        }
        ShoeMenu();
        items.menu_Button.onClick.AddListener(ShoeMenu);
        UpdateUI();
        player.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        CreatBullerails();
        uIManager = FindObjectOfType<UIManager>();
       
    }
    public void Play()
    {          
        uIManager.ShowUI(Game.HUD);
        StartCoroutine(LatePlay());
        player.SetActive(true);
        MusicManager.PauseMusic(0.1f);
    }
    IEnumerator LatePlay()
    {
        yield return new WaitForSeconds(2.5f);
        items.StartPlay.SetActive(true);
        items.number.text = "1";
        yield return new WaitForSeconds(1f);
        items.number.text = "2";
        yield return new WaitForSeconds(1f);
        items.number.text = "3";
        yield return new WaitForSeconds(3f);
        items.StartPlay.SetActive(false);
        Game.gameStatus = Game.GameStatus.isPlaying;
        MusicManager.PlayMusic("Gameloop-16");
        SettingWalkJoystic();
    }
    void CreatBullerails()
    {
        GameObject btParent = new GameObject();
        for (int i = 0; i < 50; i++)
        {
            GameObject bt = Instantiate(items.bulletTrail);
            bt.transform.parent = btParent.transform;
            bulletTrails.Add(bt);           
        }
        for (int i = 0; i < bulletTrails.Count; i++)
        {
            bulletTrails[i].SetActive(false);
        }
    }
    public void ShowExtraHealthSlider(bool value ,EnemyHealth enemyHealth)
    {
        
         if( value == false)
        {
            items.extraHealthSlider.SetActive(false);
        }
        else       
        {
            items.extraHealthSlider.SetActive(true);
            items.extraHealthfill.fillAmount = Mathf.Clamp(enemyHealth.health / enemyHealth.totalHealth, 0, 1f);
        }

    }
    public static bool onWeaponTriggerEnter;
    public void PickupButton(bool value)
    {
        if (value == true)
        {
            items.pickupButton.GetComponent<Animator>().SetBool("open", true);
            items.pickupButton.GetComponent<Animator>().SetBool("close", false);
            if (!MusicManager.sfxAudio.isPlaying && onWeaponTriggerEnter)
            {
                MusicManager.PlaySfx("inter");
            }
           
        }
        else
        {
            items.pickupButton.GetComponent<Animator>().SetBool("open", false);
            items.pickupButton.GetComponent<Animator>().SetBool("close", true);            
            MusicManager.PlaySfx("Interface");                      
        }
      
    }
    bool show=true;
    public void ShoeMenu()
    {
        show = !show;
        if (show == true)
        {
            items.menu.GetComponent<Animator>().SetBool("open", true);
            items.menu.GetComponent<Animator>().SetBool("close", false);
            items.menu_Button.GetComponent<Image>().sprite = items.menu_sprites[1];
            MusicManager.PlaySfx("inter");
        }
        else
        {
            items.menu.GetComponent<Animator>().SetBool("open", false);
            items.menu.GetComponent<Animator>().SetBool("close", true);
            items.menu_Button.GetComponent<Image>().sprite = items.menu_sprites[0];
            MusicManager.PlaySfx("Interface");
        }       
       

    }
    int btIndex;
    public void ActiveBulletTrail(Transform pos)
    {
        bulletTrails[btIndex].SetActive(true);
        bulletTrails[btIndex].transform.position = pos.position;
        bulletTrails[btIndex].transform.rotation = pos.rotation;
        btIndex++;
        if(btIndex>= bulletTrails.Count)
        {
            btIndex = 0;
        }
    }
    public void UpdateUI()
    {
        items.scoreText  .text     = Game.currentScore.ToString();
        items.coinText   .text     = Game.TotalCoins.ToString();
        items.diemondText.text     = Game.TotalDiemonds.ToString();
        items.lifeText   .text     = Game.Life.ToString();
    }
    public void ShowKill()
    {
        items.killEffect.SetActive(true);
    }

    public void ShowPowerup(string count)
    {
        SetActive(items.powerup_Texts[pwTIndex], true, 0);
        items.powerup_Texts[pwTIndex].GetComponent<Text>().text = count;
        pwTIndex++;
        if(pwTIndex>= items.powerup_Texts.Length)
        {
            pwTIndex = 0;
        }
    }
    public void SetActive(GameObject Object,  bool value,float wait)
    {
        StartCoroutine(Active(Object, value, wait));
    }
    IEnumerator Active(GameObject Object, bool value, float wait)
    {
        yield return new WaitForSeconds(wait);
        Object.SetActive(value);
    }
   
    public void DestroyCurrentBuilding()
    {
        if (currentBuilding)
        {
            Destroy(currentBuilding);
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        UpdateUI();
    }
    #region Joystick Setting
    public FixedJoystick Joystick_Left;
    public FixedJoystick Joystick_Right;
    public static FixedJoystick walk_Joystick;
    public static FixedJoystick rotate_Joystick;
    public static int Joystick
    {
        get { return PlayerPrefs.GetInt("Joystick", 0); }
        set { PlayerPrefs.SetInt("Joystick", value); }
    }
    public void MirrorJoystick()
    {
        SetKey();
    }
    bool left=true;
    bool SetKey()
    {       
        // Save boolean using PlayerPrefs
        PlayerPrefs.SetInt("left", left ? 1 : 0);
        // Get boolean using PlayerPrefs
        left = PlayerPrefs.GetInt("foo") == 1 ? true : false;
        SettingWalkJoystic();
        return left;
    }
     void SettingWalkJoystic()
    {        
       
        if (left)
        {
            walk_Joystick = Joystick_Left;
            rotate_Joystick = Joystick_Right;
        }
        else
        {
          walk_Joystick = Joystick_Right;
          rotate_Joystick = Joystick_Left;
        }
        uIManager.ShowPopup  ("You Have Changed Joysticks");

    }


    #endregion
}
