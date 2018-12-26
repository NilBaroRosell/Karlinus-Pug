using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class kill_cono_vision : MonoBehaviour {

    private int angulo = 140;
    private int w_ref = 40;
    private int rango = 175;

    MeshFilter meshFilter;
    Vector3 oldPosition;
    Quaternion oldRotation;
    Vector3 oldScale;

    static Animator anim;
    private bool auxPressed;
    public static bool returnPlayer;
    private GameObject player;
    private GameObject target;
    private Renderer targetRenderer;
    private csAreaVision targetState;
    private liquidState liquidKill;
    private movement playerMovement;
    private Vector3 killTargetPos;
    private Vector3 playerPos;
    private float altura;
    private bool aproaching;
    private bool stuck;
    private float stuckReference;
    enum killState { WATCHING, APROACHING, KILLING, RETURNING };
    killState actualState;
    private float ghostRef;

    //Nav Mesh
    NavMeshAgent liquidAgent;
    public Material[] textures;

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
        stuckReference = 0.0f;
        stuck = false;
        auxPressed = false;
        targetRenderer = null;
        targetState = null;
        player = GameObject.Find("Jugador");
        liquidKill = player.GetComponent<liquidState>();
        playerMovement = player.GetComponent<movement>();
        liquidAgent = player.GetComponent<NavMeshAgent>();
        if (liquidAgent == null)
        {
            Debug.LogError("Nav Mesh error");
        }
        liquidAgent.enabled = false;
        target = null;
        ghostRef = Time.realtimeSinceStartup;
        actualState = killState.WATCHING;
        altura = 0;
        aproaching = false;
        killTargetPos = new Vector3(0.0f, 0.0f, 0.0f);
        meshFilter = transform.GetComponent<MeshFilter>();
        meshFilter.mesh = Cono();
        initialPosition = meshFilter.mesh.vertices;
        initialUV = meshFilter.mesh.uv;
        anim = player.GetComponent<Animator>();
        returnPlayer = false;
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
                if (hit.transform.gameObject.tag == "enemy")
                {
                    ghostRef = Time.realtimeSinceStartup;
                    target = hit.transform.gameObject;
                    target.transform.GetChild(4).gameObject.SetActive(true);
                    targetRenderer = target.transform.GetChild(4).gameObject.GetComponent<Renderer>();
                    targetState = target.GetComponent<csAreaVision>();
                    if (liquidState.hidratation > 0 && targetState.canBeKilled())
                    {
                        targetRenderer.material = textures[0];

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            playerPos = player.transform.position;
                            playerMovement.state = movement.playerState.HITTING;
                            liquidAgent.enabled = true;
                            liquidAgent.SetDestination(target.transform.GetChild(4).gameObject.transform.position);
                            player.GetComponent<Rigidbody>().useGravity = false;
                            target.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                            player.GetComponent<Collider>().enabled = false;
                            target.GetComponent<Collider>().enabled = false;
                            target.GetComponent<Rigidbody>().useGravity = false;
                            anim.SetBool("Is_Damaging", true);
                            target.GetComponent<NavMeshAgent>().enabled = false;
                            target.GetComponent<NavMeshObstacle>().enabled = true;
                            target.transform.GetChild(4).gameObject.SetActive(false);
                            stuckReference = Time.realtimeSinceStartup;
                            liquidKill.firstFrameNormal = false;
                            liquidKill.cooldown = false;
                            liquidKill.showLiquid();
                            actualState = killState.APROACHING;
                            killTargetPos = target.transform.position;
                        }
                    }
                    else targetRenderer.material = textures[1];
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

    private void Update()
    {
        switch(actualState)
        {
            case killState.WATCHING:
                if (anim.GetBool("Is_Detected")) kill_vision();
                if(!anim.GetBool("Is_Damaging")) draw_Weapon();
                break;
            case killState.APROACHING:
                if (liquidAgent.remainingDistance <= 0.2f || stuckReference + 2.5f < Time.realtimeSinceStartup) aproachEnemy(killTargetPos);
                break;
            case killState.KILLING:
                if (returnPlayer) setReturn();
                break;
            case killState.RETURNING:
                if (liquidAgent.remainingDistance <= 0.1f || stuckReference + 2.5f < Time.realtimeSinceStartup) returnToPosition();
                break;
            default:
                break;
        }
    }
    public void kill_vision()
    {
        meshFilter.mesh = areaMesh(meshFilter.mesh);
        CheckGhost();
    }

    private void aproachEnemy(Vector3 destination)
    {
        liquidKill.hideLiquid();
        //player.transform.position = target.transform.GetChild(4).transform.position;
        player.GetComponent<Rigidbody>().transform.LookAt(destination);
        actualState = killState.KILLING;
        liquidAgent.enabled = false;
        //StartCoroutine(ExecuteAfterTime(1.0f));
        aproaching = false;
        player.GetComponent<Rigidbody>().velocity *= 0;
        target.gameObject.GetComponent<Animator>().SetTrigger("Is_Dying");
        targetState.dead = true;
        killEnemy();
    }

    private void killEnemy()
    {
        
        anim.SetBool("Is_Running", false);
            anim.SetBool("Is_Crouching", false);
            anim.SetBool("Is_Walking", false);
            anim.SetBool("Is_Idle", false);
            anim.SetTrigger("Is_Hitting");
    }

    private void returnToPosition()
    {
        player.GetComponent<Collider>().enabled = true;
        player.GetComponent<Rigidbody>().useGravity = true;
        liquidKill.setHidratation();
        liquidKill.hideLiquid();
        playerMovement.state = movement.playerState.IDLE;
        liquidAgent.enabled = false;
        actualState = killState.WATCHING;
    }

    void CheckGhost()
    {
        if(target != null && ghostRef + 0.1f < Time.realtimeSinceStartup)
        target.transform.GetChild(4).gameObject.SetActive(false);
    }
    public void finishAnim(int message)
    {
        if (message == 1)
        {
            anim.SetBool("Is_Damaging", false);
            actualState = killState.RETURNING;
            stuckReference = Time.realtimeSinceStartup;
            liquidKill.showLiquid();
            liquidAgent.enabled = true;
            liquidAgent.SetDestination(playerPos);
        }

    }

    private void draw_Weapon()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !auxPressed && anim.GetBool("Is_Draw"))
        {
            auxPressed = true;
            if (anim.GetBool("Is_Detected"))
            {
                anim.SetBool("Is_Detected", false);
                anim.ResetTrigger("Is_Sheathing");
                anim.SetTrigger("Is_Sheathing");
            }
            else if (!anim.GetBool("Is_Detected"))
            {
                anim.SetBool("Is_Detected", true);
                anim.ResetTrigger("Is_Withdrawing");
                anim.SetTrigger("Is_Withdrawing");
            }
        }
        else if (!Input.GetKeyDown(KeyCode.Mouse1)) auxPressed = false;
    }


    void setReturn()
    {
      //  yield return new WaitForSeconds(time);

        anim.SetBool("Is_Damaging", false);
        actualState = killState.RETURNING;
        stuckReference = Time.realtimeSinceStartup;
        liquidKill.showLiquid();
        liquidAgent.enabled = true;
        liquidAgent.SetDestination(playerPos);
        returnPlayer = false;
    }
}
