using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class NPC : MonoBehaviour
{

    private int angulo = 140;
    private int w_ref = 40;
    private int rango = 30;

    MeshFilter meshFilter;
    csAreaVision disabler;
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
    private movement playerMovement;
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
    private Vector3 playerDist;
    private Vector3 stuckPos;
    private bool discovered;
    private bool searchingState;
    private double scaredRef;
    private double discoveredRef;
    private double searchingRef;
    private double atackRef;
    private bool atackRefTaken;
    private bool atacking;
    Renderer alertRend;
    private bool hittingEnemy = false;
    private bool sneaky = false;
    public bool dead = false;
    private float canAtackRef;
    public AudioClip catSound;
    AudioSource source;
    private bool first = true;
    private bool scared = false;

    //Nav Mesh
    NavMeshAgent enemyAgent;

    Vector3[] initialPosition;
    Vector2[] initialUV;

    // Use this for initialization
    void Awake()
    {
        GetComponent<NavMeshObstacle>().enabled = false;
        disabler = GetComponent<csAreaVision>();
        if (GameObject.Find("EnemyManager") != null) maxDist = GameObject.Find("EnemyManager").GetComponent<EnemyManager>().maxDist;
        else maxDist = 100;
        canAtackRef = 0.0f;
        stuckPos = Vector3.zero;
        Physics.IgnoreLayerCollision(9, 8);
        meshFilter = transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshFilter>();
        initialPosition = meshFilter.mesh.vertices;
        initialUV = meshFilter.mesh.uv;
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
            objectPoint[i].SetActive(false);
        }
        getDestination();
        transform.GetChild(4).gameObject.transform.position = new Vector3(0.15f, 0.023f, -0.7f) * -1 + transform.position;
        transform.GetChild(4).gameObject.SetActive(false);
        KarlinusEspectre.transform.position = new Vector3(0.15f, 0.023f, -0.7f) * -1 + transform.position;
        KarlinusEspectre.SetActive(false);
        Pepino = GameObject.Find("Pepino");
        playerMovement = GameObject.Find("Jugador").GetComponent<movement>();
        playerDist = new Vector3(GameObject.Find("Jugador").transform.position.x - rb.transform.position.x, 0.0f, GameObject.Find("Jugador").transform.position.z - rb.transform.position.z);
        discovered = false;
        discoveredRef = Time.realtimeSinceStartup;
        scaredRef = Time.realtimeSinceStartup;
        searchingRef = Time.realtimeSinceStartup;
        atackRef = Time.realtimeSinceStartup;
        lastSeenPosition = new Vector3(0.0f, 0.0f, 0.0f);
        alertRend = transform.GetChild(3).GetComponent<Renderer>();
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

    Mesh areaMesh(Mesh mesh)
    {

        Mesh _mesh = new Mesh();
        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        Vector2[] uv = new Vector2[mesh.uv.Length];

        Vector3 center = transform.localToWorldMatrix.MultiplyPoint3x4(initialPosition[0]);
        uv[0] = initialUV[0];
        Vector3 worldPoint;

        RaycastHit hit = new RaycastHit();

        for (int i = 1; i < vertices.Length; i++)
        {

            worldPoint = transform.localToWorldMatrix.MultiplyPoint3x4(initialPosition[i]);

            if (Physics.Linecast(center, worldPoint, out hit))
            {
                if (hit.transform.gameObject.tag == "cucumber" && !scared)
                {
                    scaredRef = Time.realtimeSinceStartup;
                    stuckPos = rb.transform.position;
                    StartCoroutine(CheckStuck(1));
                    scared = true;
                    getPanicDestination();
                }
                if (hit.transform.gameObject.tag == "Player")
                {
                    discovered = true;
                    lastSeenPosition = GameObject.Find("Jugador").transform.position;
                }

                if (hit.transform.position != transform.position)
                {
                    vertices[i] = transform.worldToLocalMatrix.MultiplyPoint3x4(hit.point);
                    uv[i] = new Vector2((rango + vertices[i].x) / (rango * 2), (rango + vertices[i].z) / (rango * 2));
                }

            }
            else
            {

                vertices[i] = initialPosition[i];
                uv[i] = initialUV[i];

            }

        }

        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.normals = mesh.normals;
        _mesh.triangles = mesh.triangles;

        return _mesh;



    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerDist = new Vector3(GameObject.Find("Jugador").transform.position.x - rb.transform.position.x, 0.0f, GameObject.Find("Jugador").transform.position.z - rb.transform.position.z);

        if (playerDist.magnitude <= maxDist)
        {
            IA_Controller();
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

        if (enemyAgent.velocity.normalized != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(enemyAgent.velocity.normalized);
        else
            rb.transform.LookAt(destinationPoint);

        discovered = false;
        vecEnemy1.Normalize();
        if (dead) speed = 0;
    }

    public bool canBeKilled()
    {
        if (actualState == enemyState.FIGHTING && canAtackRef + 2.0f > Time.realtimeSinceStartup && !playerAnim.GetBool("Is_Running"))
            return true;
        else if (actualState != enemyState.FIGHTING) return true;
        return false;
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

    private void getDestination()
    {
        destinationPoint = this.gameObject.GetComponent<RandomDestination>().RandomNavmeshLocation(this.gameObject, 75);
    }

    IEnumerator CheckStuck(float time)
    {
        yield return new WaitForSeconds(time);

        stuckPos = new Vector3(stuckPos.x - rb.transform.position.x, 0.0f, stuckPos.z - rb.transform.position.z);
        if (stuckPos.magnitude < 2) getPanicDestination();
        stuckPos = transform.position;
        if (actualState == enemyState.LEAVING) StartCoroutine(CheckStuck(1));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        playerMovement.state = movement.playerState.IDLE;
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
        transform.gameObject.SetActive(false);
    }

    IEnumerator playerDeath(float time)
    {
        yield return new WaitForSeconds(time);

        loadScreen.Instancia.CargarEscena("DEAD");
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
                if (actualState != enemyState.FIGHTING) meshFilter.mesh = areaMesh(meshFilter.mesh);
            }
        }
        switch (actualState)
        {
            case enemyState.PATROLLING:
                if (vecEnemy1.magnitude < 1)
                {
                    getDestination();
                }
                break;
        }
    }
}