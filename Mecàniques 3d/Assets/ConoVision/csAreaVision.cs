using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class csAreaVision : MonoBehaviour {

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
    public Vector3 playerDist;
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


    Mesh Cono() {

        Mesh _cono = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();

        Vector3 oldPosition, temp;
        oldPosition = temp = Vector3.zero;

        vertices.Add(Vector3.zero);
        normals.Add(Vector3.up);
        uv.Add(Vector2.one * 0.5f);

        int w, s;
        for (w = w_ref; w < angulo; w++) {

            for (s = 0; s < rango; s++) {
                temp.x = Mathf.Cos(Mathf.Deg2Rad * w + Mathf.Deg2Rad * (s / rango)) * rango;
                temp.z = Mathf.Sin(Mathf.Deg2Rad * w + Mathf.Deg2Rad * (s / rango)) * rango;

                if (oldPosition != temp) {

                    oldPosition = temp;
                    vertices.Add(new Vector3(temp.x, temp.y, temp.z));
                    normals.Add(Vector3.up);
                    uv.Add(new Vector2((rango + temp.x) / (rango * 2), (rango + temp.z) / (rango * 2)));

                }

            }

        }

        int[] triangles = new int[(vertices.Count - 2) * 3];
        s = 0;

        for (w = 1; w < (vertices.Count - 2); w++) {

            triangles[s++] = w + 1;
            triangles[s++] = w;
            triangles[s++] = 0;

        }

        _cono.vertices = vertices.ToArray();
        _cono.normals = normals.ToArray();
        _cono.uv = uv.ToArray();
        _cono.triangles = triangles;

        return _cono;

    }

    Vector3[] initialPosition;
    Vector2[] initialUV;

    // Use this for initialization
    void Awake() {
        GetComponent<NavMeshObstacle>().enabled = false;
        disabler = GetComponent<csAreaVision>();
        if (GameObject.Find("EnemyManager") != null) maxDist = GameObject.Find("EnemyManager").GetComponent<EnemyManager>().maxDist;
        else maxDist = 100;
        canAtackRef = 0.0f;
        stuckPos = Vector3.zero;
        Physics.IgnoreLayerCollision(9, 8);
        if (GetComponent<AudioListener>() == null) gameObject.AddComponent<AudioSource>();
        meshFilter = transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshFilter>();
        meshFilter.mesh = Cono();
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
        destinationPoint = Points[patrollingIndex];
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
                    else if (Input.GetKeyDown(KeyCode.Q) && liquidState.hidratation >= 0 && !GameObject.Find("Jugador").GetComponent<liquidState>().cooldown)
                    {
                        StartCoroutine(CheckStuck(10.0f));
                        playerScaped();
                        GameObject.Find("Jugador").GetComponent<movement>().liquidTransformation();
                        for (int i = 0; i < EnemyManager.Enemies.Length; i++)
                        {
                            if (EnemyManager.Enemies[i].GetComponent<csAreaVision>().actualState == enemyState.FIGHTING) EnemyManager.Enemies[i].GetComponent<csAreaVision>().playerScaped();
                        }
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

    Mesh areaMesh(Mesh mesh) {

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
                    getPanicDestination();
                    StartCoroutine(CheckStuck(1));
                    scared = true;
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
    void FixedUpdate() {
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
            actualState = enemyState.PATROLLING;
            lastState = enemyState.SEARCHING;
            speed = 10;
            alertRend.material.SetColor("_Color", Color.green);
            KarlinusEspectre.transform.position = new Vector3(0.15f, 0.023f, -0.7f) * -1 + transform.position;
            KarlinusEspectre.SetActive(false);
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        playerMovement.state = movement.playerState.IDLE;

        DestroyEnemy();
    }

    IEnumerator playerDeath(float time)
    {
        yield return new WaitForSeconds(time);

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
        transform.gameObject.SetActive(false);
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
                if (playerAnim.GetBool("Is_Damaging") && GetComponent<Collider>().enabled == false) speed = 0;
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
                    alertRend.material.SetColor("_Color", Color.yellow);
                    destinationPoint = GameObject.Find("Jugador").transform.position;
                    KarlinusEspectre.SetActive(false);
                }
                actualString = "P";
                break;
            case enemyState.SEARCHING:
                //if (lastState == enemyState.PATROLLING) lastSeenPosition = GameObject.Find("Jugador").transform.position;
                if (playerAnim.GetBool("Is_Damaging") && GetComponent<Collider>().enabled == false) speed = 0;
                KarlinusEspectre.transform.position = destinationPoint;
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
                if (vecEnemy1.magnitude< 1 && discovered == false)//Change to PATROLLING
                {
                    actualState = enemyState.PATROLLING;
                    lastState = enemyState.SEARCHING;
                    speed = 10;
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
                destinationPoint = GameObject.Find("Jugador").transform.position;
                if (((playerDist.magnitude <= rango / 2 || searchingRef + 5.0f < Time.realtimeSinceStartup) &&
                    lastState == enemyState.PATROLLING) || lastState == enemyState.SEARCHING && speed == 50)//Change to FIGHTING
                {
                    canAtackRef = Time.realtimeSinceStartup;
                    actualState = enemyState.FIGHTING;
                    lastState = enemyState.DETECTING;
                    speed = 50;
                    alertRend.material.SetColor("_Color", Color.red);
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
                    KarlinusEspectre.transform.position = lastSeenPosition;
                }
                actualString = "D";
                break;
            case enemyState.FIGHTING:
                if (first)
                {
                    source.clip = catSound;
                    source.Play();
                    first = false;
                }
                destinationPoint = GameObject.Find("Jugador").transform.position;
                if (canBeKilled() == false)
                {
                    if (playerDist.magnitude< 1.5f)
                    {
                        speed = 0;
                        playerMovement.state = movement.playerState.HITTING;
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
                        KarlinusEspectre.SetActive(true);
                        KarlinusEspectre.transform.position = lastSeenPosition;
                    }
                }
                else if (playerAnim.GetBool("Is_Damaging") && GetComponent<Collider>().enabled == false) speed = 0;
                actualString = "F";
                break;
            case enemyState.LEAVING:
                alertRend.material.SetColor("_Color", Color.blue);
                if ((vecEnemy1.magnitude< 1 || scaredRef + 15.0f < Time.realtimeSinceStartup) && scaredRef + 10.0f < Time.realtimeSinceStartup)
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


