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
}

public class GameController_Grappling : MonoBehaviour
{
    public MyItems items;
    public static GameController_Grappling Instace;
    public static bool isWeaponActivated;
    public GameObject currentBuilding;
    int pwTIndex;
    [HideInInspector] public List<GameObject> bulletTrails;
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
    public void PickupButton(bool value)
    {
        if (value == true)
        {
            items.pickupButton.GetComponent<Animator>().SetBool("open", true);
            items.pickupButton.GetComponent<Animator>().SetBool("close", false);
        }
        else
        {
            items.pickupButton.GetComponent<Animator>().SetBool("open", false);
            items.pickupButton.GetComponent<Animator>().SetBool("close", true);
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
        }
        else
        {
            items.menu.GetComponent<Animator>().SetBool("open", false);
            items.menu.GetComponent<Animator>().SetBool("close", true);
            items.menu_Button.GetComponent<Image>().sprite = items.menu_sprites[0];
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

    public void ShowPowerup(int count)
    {
        SetActive(items.powerup_Texts[pwTIndex], true, 0);
        items.powerup_Texts[pwTIndex].GetComponent<Text>().text = count.ToString();
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
    // Start is called before the first frame update
    void Start()
    {
        CreatBullerails();
    }
    public void DestroyCurrentBuilding()
    {
        if (currentBuilding)
        {
            Destroy(currentBuilding);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
