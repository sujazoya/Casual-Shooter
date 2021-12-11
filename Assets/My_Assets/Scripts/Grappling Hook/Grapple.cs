using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] float pullSpeed = 0.5f;
    [SerializeField] float stopDistance = 4f;
    [SerializeField] GameObject hookPrefab;
    public Transform shootTransform;
    Rigidbody rigid;
    Hook hook;
    public bool pulling;
    GameController_Grappling gameController;
    public Transform target;
    Animator playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        pulling = false;
        gameController = FindObjectOfType<GameController_Grappling>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(hook == null && Input.GetMouseButtonDown(0) && target)
        //{
        //    StopAllCoroutines();
        //    pulling = false;
        //    transform.LookAt(GetTargetPos());
        //    hook = Instantiate(hookPrefab, shootTransform.position, Quaternion.identity).GetComponent<Hook>();
        //    hook.Initialize(this, target);
        //    //playerAnimator.SetTrigger("hook");
        //    StartCoroutine(DestroyHookAfterLifetime());
        //}
        //else if(hook != null && Input.GetMouseButtonDown(1))
        //{
        //    DestroyHook();
        //}

        if (!pulling || hook == null) return;

        if(Vector3.Distance(transform.position, hook.transform.position) <= stopDistance)
        {
            DestroyHook();
        }
        else
        {           
            int i = 65;
            StartLerping(i);
        }
       
    } 
    public void CreateGrapple(Transform myTarget)
    {
        if (hook)
            return;
        target = myTarget;
        StopAllCoroutines();
        pulling = false;
        transform.LookAt(GetTargetPos());
        hook = Instantiate(hookPrefab, shootTransform.position, Quaternion.identity).GetComponent<Hook>();
        hook.Initialize(this, myTarget);
        //playerAnimator.SetTrigger("hook");
        StartCoroutine(DestroyHookAfterLifetime());
    }
    private Vector3 grapPoint;
    Vector3 GetTargetPos()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(camRay, out hit, 1000))
        {
            grapPoint = hit.point;
        }
        return grapPoint;
    }

    public void StartPull()
    {
        pulling = true;
    }

    public void DestroyHook()
    {
        if (hook == null) return;

        pulling = false;
        Destroy(hook.gameObject);
        hook = null;
        gameController.DestroyCurrentBuilding();
        gameController.currentBuilding = target.transform.parent.gameObject;
        //target.gameObject.SetActive(false);
        target = null;
        _isLerping = false;
        //playerAnimator.SetTrigger("idle");

    }

    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(8f);

        DestroyHook();
    }

    public float timeTakenDuringLerp = 2f;

    private bool _isLerping;

    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private float _timeStartedLerping;

    void StartLerping(int i)
    {
        _isLerping = true;
        _timeStartedLerping = Time.time; // adding 1 to time.time here makes it wait for 1 sec before starting

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        _startPosition = transform.position;
        _endPosition = hook.transform.position;
        //playerAnimator.SetTrigger("pull");


    }
    public float speed = 5;
    //We do the actual interpolation in FixedUpdate(), since we're dealing with a rigidbody
    void FixedUpdate()
    {
        if (_isLerping)
        {
            //We want percentage = 0.0 when Time.time = _timeStartedLerping
            //and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
            //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
            //"Time.time - _timeStartedLerping" is.
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            //Perform the actual lerping.  Notice that the first two parameters will always be the same
            //throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
            //to start another lerp)
            transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete* speed);

            //When we've completed the lerp, we set _isLerping to false
            if (percentageComplete >= 1f)
            {
                _isLerping = false;
                //DestroyHook();
            }
        }
    }

}


