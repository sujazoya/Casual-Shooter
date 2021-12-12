using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Soldier : MonoBehaviour
{
    public Transform player;
    public Transform head;
    [SerializeField]
    Animator anim;
    bool parsuing = false;

    string state = "patrol";
    public Transform[] waypoints;
    int currentWp = 0;
    [SerializeField]
    float rotSpeed = 0.5f;
    [SerializeField]
    float speed = 1.5f;
    [SerializeField]
    float accuracyWP = 5.0f;    
    // public WeaponLauncher WeaponLists;
    //  public Text myText;
    public float attackDistance;
    [SerializeField] float waitBeforeShot=1;
    [SerializeField] GameObject muzzle;
    GameController_Grappling gameController;
    [SerializeField] GameObject lazer;
    public LayerMask playerMask;
    public float damage = 5;
    // private SniperManager sniperManager;
    // Start is called before the first frame update
    void Awake()
    {
      
    }
    //public void Damage(float damage)
    //{
    //    if (sniperManager)
    //        sniperManager.Damage(damage);
    //    //Debug.Log("Damage Called" + transform.name);
    //}
    void Start()
    {
        // waypoints_Handler = GameObject.FindGameObjectWithTag("Waypoint").GetComponent<waypoints_Handler>();
        //if (waypoints_Handler) { waypoints = waypoints_Handler.waypoints; }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        gameController=FindObjectOfType<GameController_Grappling>();
        if (muzzle) { muzzle.SetActive(false); }
        if (lazer) { lazer.SetActive(false); }
        //  player = GameObject.FindGameObjectWithTag("Player").transform;        
    }
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // waypoints = waypoints_Handler.wayPoints;
        // anim.SetBool("Idle", true);
    }
    // Update is called once per frame
    void Update()
    {
        // player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 direction = player.position - this.transform.position;
        direction.y = 0;
        float angle = Vector3.Angle(direction, head.up);

        if (state == "patrol" && waypoints.Length > 0)
        {
            if (anim)
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Walk", true);
            }

            if (Vector3.Distance(waypoints[currentWp].transform.position, transform.position) < accuracyWP)
            {
                //currentWp = Random.Range(0, waypoints.Length);

                currentWp++;
                if (currentWp >= waypoints.Length)
                {
                    currentWp = 0;
                }
            }
            //rotate towards point
            direction = waypoints[currentWp].transform.position - transform.position;
            this.transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            this.transform.Translate(0, 0, Time.deltaTime * speed);
        }
        if (Vector3.Distance(player.position, this.transform.position) < attackDistance)//&& angle < 30 || state =="parsuing")
        {
            state = "Run";
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            if (direction.magnitude > 90)
            {
                this.transform.Translate(0, 0, Time.deltaTime * speed);
                if (anim)
                {
                    anim.SetBool("Walk", true);
                    //anim.SetBool("Attack", false);
                }

            }
            else
            {
                if (anim)
                {
                    anim.SetBool("Walk", false);
                    //anim.SetBool("Attack", true);
                }                         
                transform.LookAt(player);
                if(Forward() && shoot&&player.GetComponent<PlayerHealth>().health>0&& Game.gameStatus == Game.GameStatus.isPlaying)
                {                   
                    StartCoroutine(Shoot());
                }
                
            }
        }
        else
        {
            if (anim)
            {
                anim.SetBool("Walk", true);
                //anim.SetBool("Attack", false);
            }

            state = "patrol";
        }
    }
    bool value;
    bool Forward()
    {
       
        Ray ray = new Ray(transform.position, transform.forward );
        RaycastHit hit = default(RaycastHit);
        // Target was hit.
        if (Physics.Raycast(ray, out hit, 500f, playerMask))
        {
            if (hit.collider.transform != this.transform)
            {
                // Handle shot effects on target.
                if (hit.collider.transform.tag == Game.playerTag)
                {
                    if (lazer) { lazer.SetActive(true); }
                    value=true ;
                }
                else
                {
                    if (lazer) { lazer.SetActive(false); }
                    value= false;
                }

                //Call the damage behaviour of target if exists.
              
            }
        }
        return value;
        //var relativePoint = transform.InverseTransformPoint(player.position);
      
    }
    bool shoot=true;
    IEnumerator Shoot()
    {
        shoot = false;
        anim.SetTrigger("shoot");
        if (muzzle) { muzzle.SetActive(true); }
        gameController.ActiveBulletTrail(muzzle.transform);
        if (player.GetComponent<PlayerHealth>())
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage, 0.7f);
        }
        yield return new WaitForSeconds(waitBeforeShot);
        shoot = true;        
       
    }
}

