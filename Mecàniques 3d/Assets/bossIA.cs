using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.Rendering;

public class bossIA : MonoBehaviour
{

    private int angulo = 100;
    private int rango = 30;

    public enum enemyState { PATROLLING, DETECTING, SEARCHING, FIGHTING, LEAVING };
    public enemyState actualState = enemyState.PATROLLING;
    public static string actualString;
    public enemyState lastState = enemyState.PATROLLING;
    Vector3 oldPosition;
    Quaternion oldRotation;
    Vector3 oldScale;
    Vector3 patrollingPosition;


    private Rigidbody rb;
    private Animator anim;
    static Animator playerAnim;
    private Controller playerMovement;
    private float maxDist;

    public int speed;

    //Patrol points and variables
    public GameObject[] objectPoint;
    private Vector3[] Points;
    private int patrollingIndex;
    public GameObject KarlinusEspectre;
    GameObject Pepino;
    public Vector3 lastSeenPosition;
    private Vector3 destinationPoint;
    private Vector3 vecEnemy1;
    private Vector3 vecCenter;
    private Vector3 rbDirection;
    public Vector3 playerDist;
    private Vector3 stuckPos;
    public static bool discovered;
    private bool searchingState;
    private double scaredRef;
    private double discoveredRef;
    private double searchingRef;
    private int seenAcum;
    private double lostRef;
    private double atackRef;
    private bool atackRefTaken;
    private bool atacking;
    Renderer alertRend;
    public bool hittingEnemy = false;
    private bool sneaky = false;
    public bool dead = false;
    private float canAtackRef;
    public AudioClip catSound;
    AudioSource source;
    private bool first = true;
    private bool scared = false;

    //Nav Mesh
    NavMeshAgent enemyAgent;

    // Use this for initialization
    void Awake()
    {
        vecCenter = Vector3.zero;
        GetComponent<NavMeshObstacle>().enabled = false;
        KarlinusEspectre = transform.GetChild(5).gameObject;
        KarlinusEspectre.transform.parent = null;
        if (GameObject.Find("EnemyManager") != null) maxDist = GameObject.Find("EnemyManager").GetComponent<EnemyManager>().maxDist;
        else maxDist = 100;
        canAtackRef = 0.0f;
        stuckPos = Vector3.zero;
        Physics.IgnoreLayerCollision(9, 8);
        if (GetComponent<AudioListener>() == null) gameObject.AddComponent<AudioSource>();
        if (GetComponent<RandomDestination>() == null) gameObject.AddComponent<RandomDestination>();
        transform.GetChild(0).gameObject.AddComponent<visibleEnemy>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        anim.SetBool("Is_Walking", true);
        atackRefTaken = false;
        atacking = false;
        searchingState = true;
        Points = new Vector3[objectPoint.Length];
        patrollingIndex = 0;
        for (int i = 0; i < objectPoint.Length; i++)
        {
            Points[i] = objectPoint[i].transform.position;
            Destroy(objectPoint[i]);
        }
        destinationPoint = Points[patrollingIndex];
        transform.GetChild(4).gameObject.transform.position = new Vector3(0.15f, 0.023f, -0.7f) * -1 + transform.position;
        transform.GetChild(4).gameObject.SetActive(false);
        KarlinusEspectre.transform.position = new Vector3(0.15f, 0.023f, -0.7f) * -1 + transform.position;
        KarlinusEspectre.SetActive(false);
        Pepino = GameObject.Find("Pepino");
        playerMovement = GameObject.Find("Jugador").GetComponent<Controller>();
        playerDist = new Vector3(GameObject.Find("Jugador").transform.position.x - rb.transform.position.x, 0.8f, GameObject.Find("Jugador").transform.position.z - rb.transform.position.z);
        discovered = false;
        discoveredRef = Time.realtimeSinceStartup;
        scaredRef = Time.realtimeSinceStartup;
        lostRef = Time.realtimeSinceStartup;
        searchingRef = Time.realtimeSinceStartup;
        atackRef = Time.realtimeSinceStartup;
        lastSeenPosition = new Vector3(0.0f, 0.0f, 0.0f);
        alertRend = transform.GetChild(3).GetComponent<MeshRenderer>();
        // alertRend.material.shader = Shader.Find("_Color");
        alertRend.material.SetColor("_Color", Color.green);
        enemyAgent = this.GetComponent<NavMeshAgent>();
        if (enemyAgent == null)
        {
            Debug.LogError("Nav Mesh error");
        }
        else enemyAgent.SetDestination(destinationPoint);
        enemyAgent.updateRotation = false;
        playerAnim = GameObject.Find("Jugador").GetComponent<Animator>();
        seenAcum = 0;
    }

    void Start()
    {
        anim.enabled = true;
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        switch (speed)
        {
            case 0:
                if (dead)
                {
                    anim.SetBool("Is_Running", false);
                    anim.SetBool("Is_Walking", false);
                    anim.SetBool("Is_Fighting", false);
                    StartCoroutine(ExecuteAfterTime(4));
                }
                else if (canBeKilled())
                {
                    anim.SetBool("Is_Running", false);
                    anim.SetBool("Is_Walking", false);
                    anim.SetBool("Is_Fighting", false);
                }
                else if (playerAnim.GetBool("Is_Damaging") == false && dead == false)
                {
                    anim.SetBool("Is_Running", false);
                    anim.SetBool("Is_Walking", false);
                    if (Time.realtimeSinceStartup > atackRef + 1)
                    {
                        StartCoroutine(playerDeath(3.5f));
                        anim.SetBool("Is_Fighting", true);
                        atacking = true;
                        playerAnim.SetBool("Is_Dying", true);
                    }
                }
                break;
            case 10:
                anim.SetBool("Is_Running", false);
                anim.SetBool("Is_Walking", true);
                anim.SetBool("Is_Fighting", false);
                atackRefTaken = false;
                atacking = false;
                break;
            case 50:
            case 80:
                anim.SetBool("Is_Running", true);
                anim.SetBool("Is_Walking", false);
                anim.SetBool("Is_Fighting", false);
                atackRefTaken = false;
                atacking = false;
                break;
            default:
                anim.SetBool("Is_Running", false);
                anim.SetBool("Is_Walking", false);
                anim.SetBool("Is_Fighting", false);
                atackRefTaken = false;
                atacking = false;
                break;
        }
    }

    void areaMesh()
    {
        Vector3 v = playerDist;
        float dist = v.sqrMagnitude;

        v.Normalize();

        float dotFov = Mathf.Cos(angulo * 0.5f * Mathf.Deg2Rad);
        float dot = Vector3.Dot(transform.forward, v);

        if (dist <= rango * rango && dot >= dotFov)
        {
            RaycastHit hit = new RaycastHit();
            Ray raycast = new Ray(transform.position, v);
            if (Physics.Raycast(raycast, out hit, rango, 1 << LayerMask.NameToLayer("cobertura")) && hit.transform.gameObject.tag == "Player")
            {
                Debug.Log(hit.transform.name);
                discovered = true;
                lastSeenPosition = GameObject.Find("Jugador").transform.position;

            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerDist = new Vector3(GameObject.Find("Jugador").transform.position.x - rb.transform.position.x, 0.8f, GameObject.Find("Jugador").transform.position.z - rb.transform.position.z);
        vecCenter = new Vector3(GameObject.Find("Box001").transform.position.x - rb.transform.position.x, 0.8f, GameObject.Find("Box001").transform.position.z - rb.transform.position.z);

            if (!hittingEnemy)
            {
            if (actualState != enemyState.PATROLLING && actualState != enemyState.FIGHTING && vecCenter.magnitude > 3.75f) speed = 0;
                IA_Controller();
                if (canBeKilled()) IsCloseAndVisible();
                else transform.GetChild(4).gameObject.SetActive(false);
            }
            Start();

        if (enemyAgent.velocity.magnitude > 0.75f)
            transform.rotation = Quaternion.LookRotation(enemyAgent.velocity.normalized);
        else if(!hittingEnemy)
            rb.transform.LookAt(destinationPoint);

        discovered = false;
        vecEnemy1.Normalize();
        if (hittingEnemy)
        {
            speed = 0;
        }
    }

    public bool canBeKilled()
    {
        if (actualState != enemyState.FIGHTING && !playerAnim.GetBool("Is_Running"))
        {
            return true;
        }
        return false;
    }

    private void IsCloseAndVisible()
    {
        if (!GetComponent<NavMeshObstacle>().enabled)
        {
            if (transform.GetChild(0).GetComponent<visibleEnemy>().visible && playerDist.magnitude <= 7.5f)
            {
                transform.GetChild(4).gameObject.SetActive(true);
                if (kill_cono_vision.assignedTargets[0] == null) kill_cono_vision.assignedTargets[0] = this.gameObject;
                else kill_cono_vision.assignedTargets[1] = this.gameObject;
            }
            else transform.GetChild(4).gameObject.SetActive(false);
        }
    }

    private void getPanicDestination()
    {
        destinationPoint = this.gameObject.GetComponent<RandomDestination>().RandomNavmeshLocation(this.gameObject, 75);
        patrollingPosition = transform.position;
        speed = 50;
        actualState = enemyState.LEAVING;
        lastState = enemyState.PATROLLING;
        alertRend.material.SetColor("_Color", Color.blue);
    }
    IEnumerator CheckStuck(float time)
    {
        yield return new WaitForSeconds(time);

        if (actualState == enemyState.LEAVING)
        {
            stuckPos = new Vector3(stuckPos.x - rb.transform.position.x, 0.0f, stuckPos.z - rb.transform.position.z);
            if (stuckPos.magnitude < 2) destinationPoint = this.gameObject.GetComponent<RandomDestination>().RandomNavmeshLocation(this.gameObject, 75);
            stuckPos = transform.position;
            StartCoroutine(CheckStuck(1));
        }
        else if (actualState == enemyState.SEARCHING)
        {
            actualState = enemyState.PATROLLING;
            lastState = enemyState.SEARCHING;
            speed = 10;
            alertRend.material.SetColor("_Color", Color.green);
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        playerMovement.state = Controller.playerState.IDLE;
        destinationPoint = Points[patrollingIndex];
    }

    IEnumerator playerDeath(float time)
    {
        yield return new WaitForSeconds(time);

        GameObject.Find("Jugador").transform.position = new Vector3(0, -500, 0);
        if (GameObject.Find("CameraBase") != null) Destroy(GameObject.Find("CameraBase").GetComponent<CameraFollow>());
        loadScreen.Instancia.CargarEscena("DEAD");
    }

    public void playerScaped()
    {
        StartCoroutine(CheckStuck(10.0f));
        destinationPoint = this.gameObject.GetComponent<RandomDestination>().RandomNavmeshLocation(this.gameObject, 75);
        patrollingPosition = transform.position;
        speed = 50;
        actualState = enemyState.SEARCHING;
        lastState = enemyState.DETECTING;
    }

    public void noiseDetected()
    {
        actualState = enemyState.DETECTING;
        searchingRef = Time.realtimeSinceStartup;
        lastState = enemyState.PATROLLING;
        alertRend.material.SetColor("_Color", Color.yellow);
        destinationPoint = GameObject.Find("Jugador").transform.position;
        KarlinusEspectre.SetActive(false);
    }

    void IA_Controller()
    {
        destinationPoint.y = transform.position.y + 0.8f;
        if (GetComponent<NavMeshObstacle>().enabled == false)
        {
            enemyAgent.SetDestination(destinationPoint);
            enemyAgent.speed = speed / 10;
        }
        else
        {
            anim.SetBool("Is_Running", false);
            anim.SetBool("Is_Walking", false);
        }
        vecEnemy1 = new Vector3(destinationPoint.x - rb.transform.position.x, 0.0f, destinationPoint.z - rb.transform.position.z);
        if (oldPosition != transform.position || oldRotation != transform.rotation || oldScale != transform.localScale)
        {

            oldPosition = transform.position;
            oldRotation = transform.rotation;
            oldScale = transform.localScale;
            if (playerDist.magnitude <= rango)
            {
                if (actualState == enemyState.PATROLLING || actualState == enemyState.SEARCHING) searchingRef = Time.realtimeSinceStartup;
                if (actualState != enemyState.FIGHTING && actualState != enemyState.LEAVING) areaMesh();
            }
        }
        switch (actualState)
        {
            case enemyState.PATROLLING:
                seenAcum = 0;
                if (playerAnim.GetBool("Is_Damaging") && GetComponent<Collider>().enabled == false) speed = 0;
                if (vecEnemy1.magnitude < 1)
                {
                    patrollingIndex++;
                    if (patrollingIndex >= Points.Length) patrollingIndex = 0;
                    destinationPoint = Points[patrollingIndex];
                }
                if (playerDist.magnitude <= rango / 3 && sneaky == false) sneaky = true;
                else if (sneaky && playerDist.magnitude > rango / 3) sneaky = false;
                if (playerDist.magnitude <= rango && discovered)//Change to DETECTING
                {
                    actualState = enemyState.DETECTING;
                    searchingRef = Time.realtimeSinceStartup;
                    lastState = enemyState.PATROLLING;
                    alertRend.material.SetColor("_Color", Color.yellow);
                    destinationPoint = GameObject.Find("Jugador").transform.position;
                    KarlinusEspectre.SetActive(false);
                }
                actualString = "P";
                break;
            case enemyState.SEARCHING:
                if (playerAnim.GetBool("Is_Damaging") && GetComponent<Collider>().enabled == false) speed = 0;
                if (discoveredRef + 0.25f < Time.realtimeSinceStartup && searchingState)
                {
                    alertRend.material.SetColor("_Color", Color.yellow);
                    discoveredRef = Time.realtimeSinceStartup;
                    searchingState = false;
                }
                else if (discoveredRef + 1.0f < Time.realtimeSinceStartup && searchingState == false)
                {
                    alertRend.material.SetColor("_Color", Color.white);
                    discoveredRef = Time.realtimeSinceStartup;
                    searchingState = true;
                }
                destinationPoint = lastSeenPosition;
                if (lostRef + 5.0f < Time.realtimeSinceStartup && discovered == false)//Change to PATROLLING
                {
                    actualState = enemyState.PATROLLING;
                    lastState = enemyState.SEARCHING;
                    speed = 10;
                    destinationPoint = Points[patrollingIndex];
                    alertRend.material.SetColor("_Color", Color.green);
                    KarlinusEspectre.transform.position = new Vector3(0.15f, 0.023f, -0.7f) * -1 + transform.position;
                    KarlinusEspectre.SetActive(false);
                }
                else if (playerDist.magnitude <= rango && discovered)//Change to DETECTING
                {
                    actualState = enemyState.DETECTING;
                    searchingRef = Time.realtimeSinceStartup;
                    lastState = enemyState.PATROLLING;
                    alertRend.material.SetColor("_Color", Color.yellow);
                    destinationPoint = GameObject.Find("Jugador").transform.position;
                    KarlinusEspectre.SetActive(false);
                }
                actualString = "S";
                break;
            case enemyState.DETECTING:
                seenAcum++;
                destinationPoint = GameObject.Find("Jugador").transform.position;
                if (seenAcum > 150 || ((playerDist.magnitude <= rango / 3 || searchingRef + 5.0f < Time.realtimeSinceStartup) &&
                    lastState == enemyState.PATROLLING) || lastState == enemyState.SEARCHING && speed == 50)//Change to FIGHTING
                {
                    canAtackRef = Time.realtimeSinceStartup;
                    actualState = enemyState.FIGHTING;
                    lastState = enemyState.DETECTING;
                    speed = 50;
                    alertRend.material.SetColor("_Color", Color.red);
                }
                else if (discovered == false)//Change to SEARCHING
                {
                    actualState = enemyState.SEARCHING;
                    lastState = enemyState.DETECTING;
                    discoveredRef = Time.realtimeSinceStartup;
                    lostRef = Time.realtimeSinceStartup;
                    lastSeenPosition = GameObject.Find("Jugador").transform.position;
                    KarlinusEspectre.SetActive(true);
                    KarlinusEspectre.transform.eulerAngles = GameObject.Find("Jugador").transform.eulerAngles + new Vector3(270, 0, 0);
                    KarlinusEspectre.transform.position = lastSeenPosition;
                }
                actualString = "D";
                break;
            case enemyState.FIGHTING:
                misions.fight = true;
                if (first)
                {
                    source.clip = catSound;
                    source.Play();
                    first = false;
                }
                destinationPoint = GameObject.Find("Jugador").transform.position;
                if (canBeKilled() == false)
                {
                    if (playerDist.magnitude < 1.5f)
                    {
                        speed = 0;
                        playerMovement.state = Controller.playerState.HITTING;
                        if (atacking == false) playerDeath(3.0f);
                        if (atackRefTaken == false)
                        {
                            atackRef = Time.realtimeSinceStartup;
                            atackRefTaken = true;
                        }
                    }
                    else if (playerDist.magnitude >= 1.5f) speed = 80;
                }
                else if (playerAnim.GetBool("Is_Damaging") && GetComponent<Collider>().enabled == false) speed = 0;
                actualString = "F";
                break;
            case enemyState.LEAVING:
                alertRend.material.SetColor("_Color", Color.blue);
                if ((vecEnemy1.magnitude < 1 || scaredRef + 15.0f < Time.realtimeSinceStartup) && scaredRef + 10.0f < Time.realtimeSinceStartup)
                {
                    StopCoroutine(CheckStuck(1));
                    actualState = enemyState.PATROLLING;
                    lastState = enemyState.LEAVING;
                    alertRend.material.SetColor("_Color", Color.green);
                    destinationPoint = patrollingPosition;
                    speed = 10;
                }
                break;
        }
    }
}


