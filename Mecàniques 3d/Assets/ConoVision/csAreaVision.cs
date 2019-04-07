using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.Rendering;

public class csAreaVision : MonoBehaviour {

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
    private Vector3 rbDirection;
    public Vector3 playerDist;
    private Vector3 stuckPos;
    private bool discovered;
    private double scaredRef;
    private double discoveredRef;
    private double searchingRef;
    private double atackRef;
    private bool atackRefTaken;
    private bool atacking;
    public bool hittingEnemy = false;
    private bool sneaky = false;
    public bool dead = false;
    private float canAtackRef;
    public AudioClip catSound;
    AudioSource source;
    private bool first = true;
    private bool scared = false;
    private int keepStateAcum;

    //Nav Mesh
    NavMeshAgent enemyAgent;

    //Sprites
    Sprite parry;

    // Use this for initialization
    void Awake() {
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
        searchingRef = Time.realtimeSinceStartup;
        atackRef = Time.realtimeSinceStartup;
        lastSeenPosition = new Vector3(0.0f, 0.0f, 0.0f);
        enemyAgent = this.GetComponent<NavMeshAgent>();
        if (enemyAgent == null)
        {
            Debug.LogError("Nav Mesh error");
        }
        else enemyAgent.SetDestination(destinationPoint);
        enemyAgent.updateRotation = false;
        playerAnim = GameObject.Find("Jugador").GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        keepStateAcum = 0;
        parry = Resources.Load<Sprite>("Sprites/ParrySprite");
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
                        GameObject.Find("Jugador").GetComponentInParent<HUD>().hideInteractSprite();
                        atacking = true;
                        playerAnim.SetBool("Is_Dying", true);
                    }
                    else if (Input.GetKeyDown(KeyCode.Q) && GameObject.Find("Jugador").GetComponent<liquidState>().hidratation >= 0 && !GameObject.Find("Jugador").GetComponent<liquidState>().cooldown)
                    {
                        StartCoroutine(CheckStuck(10.0f));
                        GameObject.Find("Jugador").GetComponentInParent<HUD>().hideInteractSprite();
                        playerScaped();
                        GameObject.Find("Jugador").GetComponent<CharacterController>().enabled = true;
                        GameObject.Find("Jugador").GetComponent<Controller>().liquidTransformation();
                        for (int i = 0; i < EnemyManager.Enemies.Length; i++)
                        {
                            if (EnemyManager.Enemies[i].GetComponent<csAreaVision>().actualState == enemyState.FIGHTING) EnemyManager.Enemies[i].GetComponent<csAreaVision>().playerScaped();
                        }
                    }
                    else if(GameObject.Find("Jugador").GetComponent<liquidState>().hidratation >= 0 && !GameObject.Find("Jugador").GetComponent<liquidState>().cooldown) GameObject.Find("Jugador").GetComponentInParent<HUD>().showInteractSprite(this.transform.position, parry);
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

    void areaMesh() {
        Vector3 v = playerDist;
        float dist = v.sqrMagnitude;

        v.Normalize();

        float dotFov = Mathf.Cos(angulo * 0.5f * Mathf.Deg2Rad);
        float dot = Vector3.Dot(transform.forward, v);

        if (dist <= rango * rango && dot >= dotFov)
        {
            RaycastHit hit = new RaycastHit();
            Ray raycast = new Ray(transform.position, v);
            if(Physics.Raycast(raycast, out hit, rango, 1 << LayerMask.NameToLayer("cobertura")) && hit.transform.gameObject.tag == "Player")
            {
                discovered = true;
                    lastSeenPosition = GameObject.Find("Jugador").transform.position;

            }
        }
        if (GameObject.Find("Pepino") != null && !scared)
            {

            v = new Vector3(GameObject.Find("Pepino").transform.position.x - rb.transform.position.x, 0.8f, GameObject.Find("Pepino").transform.position.z - rb.transform.position.z);
            dist = v.sqrMagnitude;

            v.Normalize();

            dotFov = Mathf.Cos(angulo * 0.5f * Mathf.Deg2Rad);
            dot = Vector3.Dot(transform.forward, v);

            if (dist <= (rango/2) * (rango/2) && dot >= dotFov)
            {
                RaycastHit hit = new RaycastHit();
                Ray raycast = new Ray(transform.position, v);
                if (Physics.Raycast(raycast, out hit, rango, 1 << LayerMask.NameToLayer("cobertura")) && hit.transform.gameObject.tag == "cucumber")
                {
                    scaredRef = Time.realtimeSinceStartup;
                    stuckPos = rb.transform.position;
                    getPanicDestination();
                    StartCoroutine(CheckStuck(1));
                    scared = true;

                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        playerDist = new Vector3(GameObject.Find("Jugador").transform.position.x - rb.transform.position.x, 0.8f, GameObject.Find("Jugador").transform.position.z - rb.transform.position.z);
        if (playerDist.magnitude <= maxDist)
        {
            if (!hittingEnemy)
            {
                IA_Controller();
                if (canBeKilled()) IsCloseAndVisible();
                else transform.GetChild(4).gameObject.SetActive(false);
            }
            Start();
        }
        else if (playerDist.magnitude > maxDist)
        {
            if (GameObject.Find("EnemyManager") != null)
            {
                for (int i = 0; i < EnemyManager.Enemies.Length; i++)
                    if (this.gameObject == EnemyManager.Enemies[i]) EnemyManager.EnemiesPos[i] = this.gameObject.transform.position;
            }
            this.gameObject.SetActive(false);
        }
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
        if ((actualState == enemyState.FIGHTING && canAtackRef + 2.0f > Time.realtimeSinceStartup && !playerAnim.GetBool("Is_Running")) || actualState != enemyState.FIGHTING)
        {
            return true;
        }
        return false;
    }

    private void IsCloseAndVisible()
    {
        if(!GetComponent<NavMeshObstacle>().enabled)
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
        else if(actualState == enemyState.SEARCHING)
        {
            GameObject.Find("Jugador").GetComponentInParent<HUD>().hideInteractSprite();
            actualState = enemyState.PATROLLING;
            lastState = enemyState.SEARCHING;
            speed = 10;
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        playerMovement.state = Controller.playerState.IDLE;

        DestroyEnemy();
    }

    IEnumerator playerDeath(float time)
    {
        yield return new WaitForSeconds(time);

        if (GameObject.Find("CameraBase") != null) Destroy(GameObject.Find("CameraBase").GetComponent<CameraFollow>());
        if (GameObject.Find("RenderManager") != null) Destroy(GameObject.Find("RenderManager"));
        misions.Instance.mainCamera.transform.LookAt(misions.Instance.Player.transform.position);
        GameObject.Find("Jugador").transform.position = new Vector3(0, -500, 0);
        loadScreen.Instancia.CargarEscena("DEAD");
    }

    public void DestroyEnemy()
    {
        if (GameObject.Find("EnemyManager") != null)
        {
            GameObject[] aux = new GameObject[EnemyManager.Enemies.Length - 1];
            Vector3[] auxPos = new Vector3[EnemyManager.EnemiesPos.Length - 1];
            int j = 0;
            for (int i = 0; i < EnemyManager.Enemies.Length; i++)
            {
                if (this.gameObject != EnemyManager.Enemies[i])
                {
                    aux[j] = EnemyManager.Enemies[i];
                    auxPos[j] = EnemyManager.EnemiesPos[i];
                    j++;
                }
            }
            EnemyManager.Enemies = aux;
            EnemyManager.EnemiesPos = auxPos;
        }
        Debug.Log(EnemyManager.Enemies.Length);
        Destroy(KarlinusEspectre);
        if (GameObject.Find("Jugador").GetComponent<HUD>().enemiesSeen != null)
        {
            for (int i = 0; i < GameObject.Find("Jugador").GetComponent<HUD>().enemiesSeen.Count; i++)
            {
                if (GameObject.Find("Jugador").GetComponent<HUD>().enemiesSeen[i].enemy == gameObject)
                {
                    Destroy(GameObject.Find("Jugador").GetComponent<HUD>().enemiesSeen[i].triangle);
                    Debug.Log("a cagar");
                    GameObject.Find("Jugador").GetComponent<HUD>().enemiesSeen.RemoveAt(i);
                    break;
                }
            }
        }
        this.gameObject.SetActive(false);
    }

    public void playerScaped()
    {
        StartCoroutine(CheckStuck(10.0f));
        GameObject.Find("Jugador").GetComponentInParent<HUD>().hideInteractSprite();
        destinationPoint = lastSeenPosition = this.gameObject.GetComponent<RandomDestination>().RandomNavmeshLocation(this.gameObject, 75);
        speed = 50;
        keepStateAcum = 10;
        actualState = enemyState.SEARCHING;
        lastState = enemyState.DETECTING;
        StartCoroutine(KeepState(Time.deltaTime, enemyState.DETECTING, enemyState.SEARCHING));
    }

    IEnumerator KeepState(float time, enemyState last, enemyState actual)
    {
        yield return new WaitForSeconds(time);

        keepStateAcum--;
        actualState = actual;
        lastState = last;
        if (keepStateAcum > 0)StartCoroutine(KeepState(Time.deltaTime, last, actual));
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
                if (vecEnemy1.magnitude< 1)
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
                    destinationPoint = GameObject.Find("Jugador").transform.position;
                    KarlinusEspectre.SetActive(false);
                }
                actualString = "P";
                break;
            case enemyState.SEARCHING:
                if (playerAnim.GetBool("Is_Damaging") && GetComponent<Collider>().enabled == false) speed = 0;
                destinationPoint = lastSeenPosition;
                if (vecEnemy1.magnitude< 1 && discovered == false)//Change to PATROLLING
                {
                    actualState = enemyState.PATROLLING;
                    lastState = enemyState.SEARCHING;
                    speed = 10;
                    KarlinusEspectre.transform.position = new Vector3(0.15f, 0.023f, -0.7f) * -1 + transform.position;
                    KarlinusEspectre.SetActive(false);
                }
                else if (playerDist.magnitude <= rango && discovered)//Change to DETECTING
                {
                    actualState = enemyState.DETECTING;
                    searchingRef = Time.realtimeSinceStartup;
                    lastState = enemyState.PATROLLING;
                    destinationPoint = GameObject.Find("Jugador").transform.position;
                    KarlinusEspectre.SetActive(false);
                }
                actualString = "S";
                break;
            case enemyState.DETECTING:
                destinationPoint = GameObject.Find("Jugador").transform.position;
                if (((playerDist.magnitude <= rango / 2 || searchingRef + 5.0f < Time.realtimeSinceStartup) &&
                    lastState == enemyState.PATROLLING) || lastState == enemyState.SEARCHING && speed == 50)//Change to FIGHTING
                {
                    canAtackRef = Time.realtimeSinceStartup;
                    actualState = enemyState.FIGHTING;
                    lastState = enemyState.DETECTING;
                    speed = 50;
                    Vector3 enemyDist;
                    for (int i = 0; i< EnemyManager.Enemies.Length; i++)
                    {
                        if (EnemyManager.Enemies[i].activeSelf && !GameObject.ReferenceEquals(EnemyManager.Enemies[i], gameObject) && EnemyManager.Enemies[i].GetComponent<csAreaVision>().actualState != enemyState.FIGHTING)
                        {
                            enemyDist = new Vector3(EnemyManager.Enemies[i].transform.position.x - rb.transform.position.x, 0.0f, EnemyManager.Enemies[i].transform.position.z - rb.transform.position.z);
                            if (enemyDist.magnitude <= 50)
                            {
                                EnemyManager.Enemies[i].GetComponent<csAreaVision>().actualState = enemyState.SEARCHING;
                                EnemyManager.Enemies[i].GetComponent<csAreaVision>().lastState = enemyState.PATROLLING;
                                EnemyManager.Enemies[i].GetComponent<csAreaVision>().lastSeenPosition = GameObject.Find("Jugador").transform.position;
                                EnemyManager.Enemies[i].GetComponent<csAreaVision>().speed = 50;
                            }
                        }
                    }
                }
                else if (discovered == false)//Change to SEARCHING
                {
                    actualState = enemyState.SEARCHING;
                    lastState = enemyState.DETECTING;
                    discoveredRef = Time.realtimeSinceStartup;
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
                    if (playerDist.magnitude > rango)//Change to SEARCHING
                    {
                        actualState = enemyState.SEARCHING;
                        lastState = enemyState.FIGHTING;
                        speed = 50;
                        lastSeenPosition = GameObject.Find("Jugador").transform.position;
                        //    KarlinusEspectre.SetActive(true);
                        //  KarlinusEspectre.transform.position = lastSeenPosition;
                    }
                }
                actualString = "F";
                break;
            case enemyState.LEAVING:
                if ((vecEnemy1.magnitude< 1 || scaredRef + 15.0f < Time.realtimeSinceStartup) && scaredRef + 10.0f < Time.realtimeSinceStartup)
                {
                    StopCoroutine(CheckStuck(1));
                    actualState = enemyState.PATROLLING;
                    lastState = enemyState.LEAVING;
                    destinationPoint = patrollingPosition;
                    speed = 10;
                }
                break;
        }
    }
}


