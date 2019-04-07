using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Servant : MonoBehaviour {

    private Rigidbody rb;
    static Animator anim;

    private bool checkPos;
    private int acumCheck;

    public int speed;

    //Patrol points and variables
    public GameObject[] objectPoint;
    public static Vector3[] Points;
    public float[] StopTime;
    public static int patrollingIndex;
    private bool stoped;
    public static Vector3 destinationPoint;
    private Vector3 vecEnemy1;

    //Nav Mesh
    NavMeshAgent enemyAgent;
    // Use this for initialization
    void Awake()
    {
        Physics.IgnoreLayerCollision(9, 8);
        if (GetComponent<AudioListener>() == null) gameObject.AddComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        Points = new Vector3[objectPoint.Length];
        patrollingIndex = 0;
        for (int i = 0; i < objectPoint.Length; i++)
        {
            Points[i] = objectPoint[i].transform.position;
            objectPoint[i].SetActive(false);
        }
        destinationPoint = Points[patrollingIndex];
        transform.GetChild(4).gameObject.transform.position = new Vector3(0.15f, 0.023f, -0.7f) * -1 + transform.position;
        transform.GetChild(4).gameObject.SetActive(false);
        stoped = true;
        if (GameObject.Find("Jugador") != null)
        {
            
            enemyAgent = this.GetComponent<NavMeshAgent>();
            if (enemyAgent == null)
            {
                Debug.LogError("Nav Mesh error");
            }
            else enemyAgent.SetDestination(destinationPoint);
            enemyAgent.updateRotation = false;
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = true;
        acumCheck = 0;
        checkPos = true;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        StartCoroutine(ExecuteAfterTime(StopTime[patrollingIndex]));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (checkPos)
        {
            acumCheck++;
            if (acumCheck > 10)
            {
                GetComponent<CapsuleCollider>().enabled = true;
                GetComponent<NavMeshAgent>().enabled = true;
                checkPos = false;
            }
        }
        if (anim == null) Start();
        IA_Controller();
        AnimController();
    }

    void IA_Controller()
    {
        destinationPoint.y = transform.position.y + 0.8f;
        vecEnemy1 = new Vector3(destinationPoint.x - rb.transform.position.x, 0.0f, destinationPoint.z - rb.transform.position.z);

            if (vecEnemy1.magnitude < 1)
        {
            patrollingIndex++;
            if (patrollingIndex >= Points.Length)
            {
                misions.nextEvent = true;
                this.gameObject.SetActive(false);
            }
            else if (patrollingIndex == 3 || patrollingIndex == 1) misions.nextEvent = true;
            destinationPoint = Points[patrollingIndex];
            Debug.Log(patrollingIndex);
            stoped = true;
            StartCoroutine(ExecuteAfterTime(StopTime[patrollingIndex]));
        }
    }

    void AnimController()
    {
        if (stoped)
        {
            enemyAgent.speed = 0;
            anim.SetBool("Walk", false);
            anim.SetBool("Backward", false);
            anim.SetBool("Run", false);
            anim.SetBool("Idle", true);
        }
        else
        {      
            switch (patrollingIndex)
            {
                case 0:
                case 1:
                case 2:
                default:
                    enemyAgent.speed = 1;
                    anim.SetBool("Walk", true);
                    anim.SetBool("Backward", false);
                    anim.SetBool("Idle", false);
                    anim.SetBool("Run", false);
                    rb.transform.LookAt(destinationPoint);
                    enemyAgent.SetDestination(destinationPoint);
                    break;

                case 4:
                case 5:
                case 6:
                case 10:
                case 11:
                case 12:
                case 15:
                case 20:
                case 23:
                    enemyAgent.speed = 3;
                    anim.SetBool("Walk", false);
                    anim.SetBool("Backward", false);
                    anim.SetBool("Idle", false);
                    anim.SetBool("Run", true);
                    rb.transform.LookAt(destinationPoint);
                    enemyAgent.SetDestination(destinationPoint);
                    break;
                case 9:
                case 14:
                case 18:
                case 21:
                case 22:
                    enemyAgent.speed = 2;
                    anim.SetBool("Walk", false);
                    anim.SetBool("Backward", true);
                    anim.SetBool("Idle", false);
                    anim.SetBool("Run", false);
                    rb.transform.LookAt(destinationPoint);
                    enemyAgent.SetDestination(destinationPoint);
                    transform.eulerAngles = transform.eulerAngles + new Vector3(0.0f, 180.0f, 0.0f);
                    break;
            }
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        stoped = false;
    }

    IEnumerator CheckStuck(float time)
    {
        yield return new WaitForSeconds(time);

        if(enemyAgent.speed == 0 || !enemyAgent.isOnNavMesh) { Awake(); Start(); }
        StartCoroutine(CheckStuck(2.0f));
    }


    public void OnLevelWasLoaded(int level)
    {
        if (level == 1) StartCoroutine(CheckStuck(2.0f));
        Start();
    }
}
