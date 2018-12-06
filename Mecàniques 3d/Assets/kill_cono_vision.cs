using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kill_cono_vision : MonoBehaviour {

    private int angulo = 140;
    private int w_ref = 40;
    private int rango = 5;

    MeshFilter meshFilter;
    Vector3 oldPosition;
    Quaternion oldRotation;
    Vector3 oldScale;

    static Animator anim;
    private GameObject player;
    private GameObject target;
    private Vector3 killTargetPos;
    private Vector3 playerPos;
    private float altura;
    private bool aproaching;
    enum killState { WATCHING, APROACHING, KILLING, RETURNING };
    killState actualState;
    private float ghostRef;


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
        player = GameObject.Find("Jugador");
        anim = player.GetComponent<Animator>();
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

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        player.GetComponent<Rigidbody>().useGravity = false;
                        player.GetComponent<Collider>().enabled = false;
                        anim.SetBool("Is_Damaging", true);
                        playerPos = player.transform.position;
                        target.transform.GetChild(4).gameObject.SetActive(false);
                        actualState = killState.APROACHING;
            //aproaching = true;
                        killTargetPos = target.transform.position;
                     }
                    //if (hit.distance <= 0.75f && aproaching)
                    //{
                    //    StartCoroutine(ExecuteAfterTime(1.25f));
                    //    aproaching = false;
                    //    player.GetComponent<Rigidbody>().velocity *= 0;
                    //    hit.transform.gameObject.GetComponent<Animator>().SetTrigger("Is_Dying");
                    //    killEnemy();
                    //}
                    //else  || aproaching)
                    // {
                    //     if (aproaching)
                    //     {
                    //         
                    //     }//aproachEnemy(killTarget);

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
                break;
            case killState.APROACHING:
                aproachEnemy(killTargetPos);
                break;
            case killState.KILLING:
                break;
            case killState.RETURNING:
                player.transform.position = playerPos;
                player.GetComponent<Collider>().enabled = true;
                player.GetComponent<Rigidbody>().useGravity = true;
                actualState = killState.WATCHING;
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
        player.transform.position = target.transform.GetChild(4).transform.position;
        player.GetComponent<Rigidbody>().transform.LookAt(destination);
        //player.GetComponent<Rigidbody>().velocity *= 0;
        //destination.Normalize();
        //player.GetComponent<Rigidbody>().AddForce(destination * 10);
        actualState = killState.KILLING;
        StartCoroutine(ExecuteAfterTime(1.0f));
        aproaching = false;
        player.GetComponent<Rigidbody>().velocity *= 0;
        target.gameObject.GetComponent<Animator>().SetTrigger("Is_Dying");
        killEnemy();
    }

    private void killEnemy()
    {
        
        anim.SetBool("Is_Running", false);
            anim.SetBool("Is_Crouching", false);
            anim.SetBool("Is_Walking", false);
            anim.SetBool("Is_Idle", false);
            anim.SetTrigger("Is_Hitting");
        target.GetComponent<Collider>().enabled = false;
    }

    void CheckGhost()
    {
        if(target != null && ghostRef + 0.1f < Time.realtimeSinceStartup)
        target.transform.GetChild(4).gameObject.SetActive(false);
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        anim.SetBool("Is_Damaging", false);
        actualState = killState.RETURNING;
    }
   
}
