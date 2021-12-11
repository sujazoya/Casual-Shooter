using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using suja;

public class Jumping_Ball : MonoBehaviour
{
    public float launchForce;
    public GameObject pointPrefab;
    public GameObject[] points;
    public int numberOfPoints;
    public LayerMask pillarLayer;
    public GameObject target;
    public float rotationSpeed;
    public Vector3 collison = Vector3.zero;
    [SerializeField] GameObject fakePillar;

    [SerializeField] GameObject pilar_Set;
    [SerializeField] private Animation roundAnimator;
    //[Header("Need 2 Maerials")]
    //[SerializeField] Material[] ballMaterials;
    [SerializeField] Animator player_Animator;
    // Start is called before the first frame update
    void Start()
    {
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = Instantiate(pointPrefab, transform.position, Quaternion.identity);
        }
        StartCoroutine(OffroundAnimator(0, false));
        fakePillar = Instantiate(fakePillar);
        fakePillar.SetActive(false);

        //PlayGame();
    }

    public void PlayGame()
    {
        MusicManager.PlayMusic("musi1");
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float Horizontal = Input.GetAxis("Horizontal");

        transform.Rotate(
            -vertical * rotationSpeed,
            Horizontal * rotationSpeed,
            0.0f
        );
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        GetTarget();
        if (target != null)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].transform.position = PointPosition(i * 0.025f);
            }
        }
       
        
    }
    void GetTarget()
    {
        var ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, pillarLayer))
        {
            target = hit.transform.gameObject;
            collison = target.transform.position;
            for (int i = 0; i < points.Length; i++)
            {
                points[i].SetActive(true);
                //GetComponent<MeshRenderer>().material = ballMaterials[1];
            }
        }
        else { 
            target = null;
            for (int i = 0; i < points.Length; i++)
            {
                points[i].SetActive(false);
                //GetComponent<MeshRenderer>().material = ballMaterials[0];
            }
        }
    }
    void HidePoints()
    {

    }
    IEnumerator OffroundAnimator(float wait, bool vatue)
    {
        yield return new WaitForSeconds(wait);
        roundAnimator.enabled=vatue;

    }
    public void NextTarget(Vector3 position)
    {
        fakePillar.SetActive(true);
        fakePillar.transform.position = position;
        StartCoroutine(OffroundAnimator(0, true));
        roundAnimator.Play();
        //StartCoroutine(OffroundAnimator(3f, false));
        StartCoroutine(ConteneuNew(position));
    }
    IEnumerator ConteneuNew(Vector3 position)
    {
        yield return new WaitForSeconds(1);
        pilar_Set.transform.position = position;       
        fakePillar.SetActive(false);
    }
    void Jump()
    {
        MusicManager.PauseMusic(0);        
        player_Animator.SetTrigger("jump");
        StartCoroutine(WaitForJump());
        //GetComponent<Rigidbody>().velocity = transform.up * 75;
    }
    IEnumerator WaitForJump()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.PlaySfx("jump");
        GetComponent<Rigidbody>().velocity = transform.forward * launchForce;
        yield return new WaitForSeconds(1.5f);
        MusicManager.UnpauseMusic();
    }
    Vector3 PointPosition(float t)
    {
        Vector3 currentPosition =(Vector3) transform.position + (collison.normalized* launchForce*t)+0.5f * Physics.gravity * (t * t);
        return currentPosition;
    }
    private void OnCollisionEnter(Collision collision)
    {
        
            //GetComponent<Rigidbody>().velocity = transform.up * 5;
               
    }
}
