using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public enum ItemType
    {
        None,Coin,Diemond,Medicare
    }
    public ItemType itemType;
    [SerializeField] GameObject effect;
    GameController_Grappling gameController;
    public void ReleaseItem()
    {
        if (itemType == ItemType.Coin)
        {
            Game.TotalCoins += Game.coinToGive;
            gameController.ShowPowerup(Game.coinToGive.ToString());
        }else
         if (itemType == ItemType.Diemond)
        {
            Game.TotalCoins += Game.diemondToGive;
            gameController.ShowPowerup(Game.diemondToGive.ToString());
        }
        else
         if (itemType == ItemType.Medicare)
        {
            Game.TotalCoins += Game.lifeToGive;
            gameController.ShowPowerup(Game.lifeToGive.ToString());
        }
        gameController.UpdateUI();
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController_Grappling>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Game.playerTag)
        {
            if (effect) {
                GameObject ef = Instantiate(effect);
                ef.transform.position = transform.position;
            }
            ReleaseItem();
        }
    }

}
