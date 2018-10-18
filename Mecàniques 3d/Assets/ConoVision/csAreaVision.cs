using UnityEngine;
using System.Collections.Generic;

public class csAreaVision : MonoBehaviour {

	public int angulo = 45;
	public int rango  = 5;

	MeshFilter meshFilter;
    enum enemyState { PATROLLING, SEARCHING, FIGHTING};
    enemyState actualState = enemyState.PATROLLING;
    enemyState lastState = enemyState.PATROLLING;
    Vector3 oldPosition;
	Quaternion oldRotation;
	Vector3 oldScale;

    private Rigidbody rb;
    static Animator anim;

    public int speed;
    private Vector3 example = new Vector3(1.0f, 0.0f, 0.0f);

    //Patrol points and variables
    public Vector3 pointA;
    public Vector3 pointB;
    private Vector3 destinationPoint;
    private bool GoToA;
    private Vector3 vecEnemy1;
    private Vector3 rbDirection;
    private double timeTurnRef;
    private Vector3 playerDist;
    private bool patrolMode;
    private bool discovered;
    private double discoveredRef;
    private double searchingRef;
    Vector3 lastSeen;
    Renderer alertRend;

    Mesh Cono(){
		
		Mesh _cono = new Mesh();
		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals  = new List<Vector3>();
		List<Vector2> uv       = new List<Vector2>();

		Vector3 oldPosition,temp;
		oldPosition = temp = Vector3.zero;
		
		vertices.Add(Vector3.zero);
		normals.Add(Vector3.up);
		uv.Add(Vector2.one*0.5f);
		
		int w,s;
		for(w=40;w<angulo;w++){
			
			for(s=0;s<rango;s++){
                temp.x = Mathf.Cos(Mathf.Deg2Rad*w+Mathf.Deg2Rad*(s/rango))*rango;
				temp.z = Mathf.Sin(Mathf.Deg2Rad*w+Mathf.Deg2Rad*(s/rango))*rango;

				if(oldPosition!=temp){
                   
					oldPosition=temp;
					vertices.Add(new Vector3(temp.x,temp.y,temp.z));
					normals.Add(Vector3.up);
					uv.Add(new Vector2((rango+temp.x)/(rango*2),(rango+temp.z)/(rango*2)));

				}

			}
			
		}
		
		int[] triangles = new int[(vertices.Count-2)*3];
		s = 0;
		
		for(w=1;w<(vertices.Count-2);w++){
			
			triangles[s++] = w+1;
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
	void Start () { 
        meshFilter = transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshFilter>();
        meshFilter.mesh = Cono();
        initialPosition = meshFilter.mesh.vertices;
		initialUV = meshFilter.mesh.uv;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        GoToA = true;
        anim.SetBool("Is_Walking", true);
        timeTurnRef = 0;
        destinationPoint = pointA;
        playerDist = new Vector3(GameObject.Find("Jugador").transform.position.x - rb.transform.position.x, 0.0f, GameObject.Find("Jugador").transform.position.z - rb.transform.position.z);
        patrolMode = true;
        discovered = false;
        discoveredRef = Time.realtimeSinceStartup;
        searchingRef = Time.realtimeSinceStartup;
        lastSeen = GameObject.Find("Jugador").transform.position;
        alertRend = transform.GetChild(3).GetComponent<Renderer>();
       // alertRend.material.shader = Shader.Find("_Color");
        alertRend.material.SetColor("_Color", Color.green);
    }

	Mesh areaMesh(Mesh mesh){
        
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
                //if(.gameObject.tag == "Jugador")
                if (hit.transform.position == GameObject.Find("Jugador").transform.position)
                {
                    discovered = true;
                    lastSeen = GameObject.Find("Jugador").transform.position;
                }
                if (hit.transform.position != GameObject.Find("Enemigo").transform.position)
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
	void FixedUpdate () { 
    playerDist = new Vector3(GameObject.Find("Jugador").transform.position.x - rb.transform.position.x, 0.0f, GameObject.Find("Jugador").transform.position.z - rb.transform.position.z);
        if (rb.velocity.magnitude > speed) rb.velocity = rb.velocity.normalized * speed;
        rb.transform.LookAt(destinationPoint);
        vecEnemy1 = new Vector3(destinationPoint.x - rb.transform.position.x, 0.0f, destinationPoint.z - rb.transform.position.z);
        if (oldPosition != transform.position || oldRotation != transform.rotation || oldScale != transform.localScale)
        {

            oldPosition = transform.position;
            oldRotation = transform.rotation;
            oldScale = transform.localScale;
            if (playerDist.magnitude <= 40 && (actualState == enemyState.PATROLLING || actualState == enemyState.SEARCHING))
            {
                meshFilter.mesh = areaMesh(meshFilter.mesh);
            }
        }
        switch (actualState)
        {
            case enemyState.PATROLLING:
                alertRend.material.SetColor("_Color", Color.green);
                if (vecEnemy1.magnitude < 1 && GoToA)
                {
                    destinationPoint = pointB;
                    GoToA = false;
                }
                else if (vecEnemy1.magnitude < 1 && GoToA == false)
                {
                    destinationPoint = pointA;
                    GoToA = true;
                }
                if (playerDist.magnitude <= 40 && discovered)
                {
                    actualState = enemyState.SEARCHING;
                    searchingRef = Time.realtimeSinceStartup;
                    lastState = enemyState.PATROLLING;                  
                }
                break;
            case enemyState.SEARCHING:
                alertRend.material.SetColor("_Color", Color.yellow);
                destinationPoint = lastSeen;
                if ((playerDist.magnitude <= 20 || searchingRef + 10 < Time.realtimeSinceStartup) && lastState == enemyState.PATROLLING && discovered)
                {
                    actualState = enemyState.FIGHTING;
                    lastState = enemyState.SEARCHING;
                    speed = 20;
                }
                else if (((discoveredRef + 10.0f <= Time.realtimeSinceStartup && lastState == enemyState.FIGHTING) || lastState == enemyState.PATROLLING) && discovered == false)
                {
                    actualState = enemyState.PATROLLING;
                    lastState = enemyState.SEARCHING;
                }
                break;
            case enemyState.FIGHTING:
                alertRend.material.SetColor("_Color", Color.red);
                destinationPoint = GameObject.Find("Jugador").transform.position;
                discoveredRef = Time.realtimeSinceStartup;
                if (playerDist.magnitude > 40)
                {
                    actualState = enemyState.SEARCHING;
                    lastState = enemyState.FIGHTING;
                    speed = 10;
                    lastSeen = GameObject.Find("Jugador").transform.position;
                }
                    break;
            default:
                break;
        }
        discovered = false;        
        vecEnemy1.Normalize();
        rb.AddForce(vecEnemy1 * speed);
        Debug.Log(actualState);
    }
    
}
