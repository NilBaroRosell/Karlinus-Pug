using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Servant : MonoBehaviour {

    private int angulo = 140;
    private int w_ref = 40;
    private int rango = 30;

    MeshFilter meshFilter;
    csAreaVision disabler;
    public static string actualString;
    Vector3 oldPosition;
    Quaternion oldRotation;
    Vector3 oldScale;
    Vector3 patrollingPosition;

    private Rigidbody rb;
    static Animator anim;
    static Animator playerAnim;
    private movement playerMovement;
    private float maxDist;

    public int speed;

    //Patrol points and variables
    public GameObject[] objectPoint;
    public static Vector3[] Points;
    public float[] StopTime;
    public static int patrollingIndex;
    private bool stoped;
    GameObject Pepino;
    public Vector3 lastSeenPosition;
    public static Vector3 destinationPoint;
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


    Mesh Cono()
    {

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
        for (w = w_ref; w < angulo; w++)
        {

            for (s = 0; s < rango; s++)
            {
                temp.x = Mathf.Cos(Mathf.Deg2Rad * w + Mathf.Deg2Rad * (s / rango)) * rango;
                temp.z = Mathf.Sin(Mathf.Deg2Rad * w + Mathf.Deg2Rad * (s / rango)) * rango;

                if (oldPosition != temp)
                {

                    oldPosition = temp;
                    vertices.Add(new Vector3(temp.x, temp.y, temp.z));
                    normals.Add(Vector3.up);
                    uv.Add(new Vector2((rango + temp.x) / (rango * 2), (rango + temp.z) / (rango * 2)));

                }

            }

        }

        int[] triangles = new int[(vertices.Count - 2) * 3];
        s = 0;

        for (w = 1; w < (vertices.Count - 2); w++)
        {

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
    void Awake()
    {
        disabler = GetComponent<csAreaVision>();
        if (GameObject.Find("EnemyManager") != null) maxDist = GameObject.Find("EnemyManager").GetComponent<EnemyManager>().maxDist;
        else maxDist = 100;
        canAtackRef = 0.0f;
        stuckPos = Vector3.zero;
        Physics.IgnoreLayerCollision(9, 8);
        if (GetComponent<AudioListener>() == null) gameObject.AddComponent<AudioSource>();
        meshFilter = transform.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshFilter>();
        meshFilter.mesh = Cono();
        initialPosition = meshFilter.mesh.vertices;
        initialUV = meshFilter.mesh.uv;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
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
        stoped = true;
        if (GameObject.Find("Jugador") != null)
        {
            playerMovement = GameObject.Find("Jugador").GetComponent<movement>();
            playerDist = new Vector3(GameObject.Find("Jugador").transform.position.x - rb.transform.position.x, 0.0f, GameObject.Find("Jugador").transform.position.z - rb.transform.position.z);
            discovered = false;
            discoveredRef = Time.realtimeSinceStartup;
            scaredRef = Time.realtimeSinceStartup;
            searchingRef = Time.realtimeSinceStartup;
            atackRef = Time.realtimeSinceStartup;
            lastSeenPosition = new Vector3(0.0f, 0.0f, 0.0f);
            alertRend = transform.GetChild(3).GetComponent<Renderer>();
            enemyAgent = this.GetComponent<NavMeshAgent>();
            if (enemyAgent == null)
            {
                Debug.LogError("Nav Mesh error");
            }
            else enemyAgent.SetDestination(destinationPoint);
            enemyAgent.updateRotation = false;
            playerAnim = GameObject.Find("Jugador").GetComponent<Animator>();
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = true;
        source = GetComponent<AudioSource>();
        StartCoroutine(ExecuteAfterTime(StopTime[patrollingIndex]));
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
                Destroy(this.gameObject);
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

    public void OnLevelWasLoaded()
    {
        Start();
    }
}
