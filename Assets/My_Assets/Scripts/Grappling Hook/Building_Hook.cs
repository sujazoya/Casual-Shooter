using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Hook : MonoBehaviour
{
    private Grapple grapple;
    // Start is called before the first frame update
    void Start()
    {
        grapple = FindObjectOfType<Grapple>();
    }
    void OnMouseDown()
    {
       
        if (grapple != null&&!GameController_Grappling.isWeaponAimed && Game.gameStatus == Game.GameStatus.isPlaying)
        {
           // grapple.target = null;
            grapple.CreateGrapple(this.transform);            
        }
    }
}
