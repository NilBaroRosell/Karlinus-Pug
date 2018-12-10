﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class csAreaVision : MonoBehaviour {

	private int angulo = 140;
    private int w_ref = 40;
	private int rango  = 30;

	MeshFilter meshFilter;
    enum enemyState { PATROLLING, DETECTING, SEARCHING, FIGHTING};
    enemyState actualState = enemyState.PATROLLING;
    enemyState lastState = enemyState.PATROLLING;
    Vector3 oldPosition;
	Quaternion oldRotation;
	Vector3 oldScale;

    private Rigidbody rb;
    static Animator anim;
    static Animator playerAnim;
    private movement playerMovement;

    public int speed;

    //Patrol points and variables
    public GameObject[] objectPoint;
    private Vector3[] Points;
    private int patrollingIndex;
    public GameObject KarlinusEspectre;
    private Vector3 lastSeenPosition;
    private Vector3 destinationPoint;
    private Vector3 vecEnemy1;
    private Vector3 rbDirection;
    private Vector3 playerDist;
    private bool discovered;
    private bool searchingState;
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

    //Nav Mesh
    NavMeshAgent enemyAgent;


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
		for(w= w_ref; w<angulo;w++){
			
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
	void Awake () {
        GetComponent<NavMeshObstacle>().enabled = false;
        canAtackRef = 0.0f;
        Physics.IgnoreLayerCollision(9, 8);
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
        playerMovement = GameObject.Find("Jugador").GetComponent<movement>();
        playerDist = new Vector3(GameObject.Find("Jugador").transform.position.x - rb.transform.position.x, 0.0f, GameObject.Find("Jugador").transform.position.z - rb.transform.position.z);
        discovered = false;
        discoveredRef = Time.realtimeSinceStartup;
        searchingRef = Time.realtimeSinceStartup;
        atackRef = Time.realtimeSinceStartup;
        lastSeenPosition = GameObject.Find("Jugador").transform.position;
        alertRend = transform.GetChild(3).GetComponent<Renderer>();
       // alertRend.material.shader = Shader.Find("_Color");
        alertRend.material.SetColor("_Color", Color.green);
        enemyAgent = this.GetComponent<NavMeshAgent>();
        if (enemyAgent == null)
        {
            Debug.LogError("Nav Mesh error");
        }
        else enemyAgent.SetDestination(destinationPoint);
        playerAnim = GameObject.Find("Jugador").GetComponent<Animator>();
    }

    void Start()
    {
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
                else if(playerAnim.GetBool("Is_Damaging") == false && dead == false)
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
            case 45:
            case 50:
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
	void FixedUpdate () { 
    playerDist = new Vector3(GameObject.Find("Jugador").transform.position.x - rb.transform.position.x, 0.0f, GameObject.Find("Jugador").transform.position.z - rb.transform.position.z);
        destinationPoint.y = transform.position.y + 0.8f;
        if (GetComponent<NavMeshObstacle>().enabled == false)
        {
            enemyAgent.SetDestination(destinationPoint);
            enemyAgent.speed = speed / 10;
            rb.transform.LookAt(destinationPoint);
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
                if(actualState == enemyState.PATROLLING || actualState == enemyState.SEARCHING) searchingRef = Time.realtimeSinceStartup;
                if(actualState != enemyState.FIGHTING) meshFilter.mesh = areaMesh(meshFilter.mesh);
            }
        }
        switch (actualState)
        {
            case enemyState.PATROLLING:
                if (playerAnim.GetBool("Is_Damaging") && GetComponent<Collider>().enabled == false) speed = 0;
                if (vecEnemy1.magnitude < 1)
                {
                    patrollingIndex++;
                    if (patrollingIndex >= Points.Length) patrollingIndex = 0;
                    destinationPoint = Points[patrollingIndex];
                }
                if (playerDist.magnitude <= rango/3 && sneaky == false)
                {
                    sneaky = true;
                    playerAnim.SetBool("Is_Detected", true);
                    playerAnim.ResetTrigger("Is_Withdrawing");
                    playerAnim.SetTrigger("Is_Withdrawing");
                }
                else if(sneaky && playerDist.magnitude > rango/3)
                {
                        sneaky = false;
                        playerAnim.SetBool("Is_Detected", false);
                        playerAnim.ResetTrigger("Is_Sheathing");
                        playerAnim.SetTrigger("Is_Sheathing");
                        playerAnim.ResetTrigger("Is_Hitting");
                }
                if (playerDist.magnitude <= rango && discovered)//Change to DETECTING
                {
                    actualState = enemyState.DETECTING;
                    searchingRef = Time.realtimeSinceStartup;
                    lastState = enemyState.PATROLLING;
                    //playerAnim.SetBool("Is_Detected", true);
                    alertRend.material.SetColor("_Color", Color.yellow);
                    destinationPoint = GameObject.Find("Jugador").transform.position;
                    //if (playerAnim.GetBool("Is_Detected") == false && sneaky == false)
                    //{
                    //    playerAnim.SetTrigger("Is_Withdrawing");
                    //}
                    KarlinusEspectre.SetActive(false);
                }
                break;
            case enemyState.SEARCHING:
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
                if (vecEnemy1.magnitude < 1 &&  discovered == false)//Change to PATROLLING
                {
                    actualState = enemyState.PATROLLING;
                    lastState = enemyState.SEARCHING;
                    playerAnim.ResetTrigger("Is_Hitting");
                    speed = 10;
                    alertRend.material.SetColor("_Color", Color.green);
                    KarlinusEspectre.transform.position = new Vector3(0.15f, 0.023f, -0.7f) * -1 + transform.position;
                    KarlinusEspectre.SetActive(false);
                    playerAnim.SetBool("Is_Detected", false);
                    playerAnim.ResetTrigger("Is_Sheathing");
                    playerAnim.SetTrigger("Is_Sheathing");
                    playerAnim.ResetTrigger("Is_Hitting");
                }
                else if(playerDist.magnitude <= rango && discovered)//Change to DETECTING
                {
                    actualState = enemyState.DETECTING;
                    searchingRef = Time.realtimeSinceStartup;
                    lastState = enemyState.PATROLLING;
                    //playerAnim.SetBool("Is_Detected", true);
                    alertRend.material.SetColor("_Color", Color.yellow);
                    destinationPoint = GameObject.Find("Jugador").transform.position;
                    KarlinusEspectre.SetActive(false);
                    playerAnim.SetBool("Is_Detected", false);
                    playerAnim.ResetTrigger("Is_Sheathing");
                    playerAnim.SetTrigger("Is_Sheathing");
                    playerAnim.ResetTrigger("Is_Hitting");
                }
                break;
            case enemyState.DETECTING:
                destinationPoint = GameObject.Find("Jugador").transform.position;
                if (((playerDist.magnitude <= rango/2 || searchingRef + 5.0f < Time.realtimeSinceStartup) && 
                    lastState == enemyState.PATROLLING) || lastState == enemyState.SEARCHING && speed == 45)//Change to FIGHTING
                {
                    canAtackRef = Time.realtimeSinceStartup;
                    actualState = enemyState.FIGHTING;
                    lastState = enemyState.DETECTING;
                    speed = 0;
                    //playerAnim.SetBool("Is_Detected", true);
                    //playerAnim.SetTrigger("Is_Sheathing");//Tendrá dos segundos de margen
                    alertRend.material.SetColor("_Color", Color.red);
                }
                else if(discovered == false)//Change to SEARCHING
                {
                    actualState = enemyState.SEARCHING;
                    lastState = enemyState.DETECTING;
                    discoveredRef = Time.realtimeSinceStartup;
                    //playerAnim.SetBool("Is_Detected", true);
                    lastSeenPosition = GameObject.Find("Jugador").transform.position;
                    KarlinusEspectre.SetActive(true);
                    KarlinusEspectre.transform.position = lastSeenPosition;
                    playerAnim.SetBool("Is_Detected", true);
                    playerAnim.ResetTrigger("Is_Withdrawing");
                    playerAnim.SetTrigger("Is_Withdrawing");
                }
                break;
            case enemyState.FIGHTING:
                destinationPoint = GameObject.Find("Jugador").transform.position;
                if (canBeKilled() == false)
                {
                    speed = 50;
                    if (playerDist.magnitude < 1.5f)
                    {
                        speed = 0;
                        playerMovement.state = movement.playerState.HITTING;
                       if (atacking == false)playerDeath(3.0f);
                        if (atackRefTaken == false)
                        {
                            atackRef = Time.realtimeSinceStartup;
                            atackRefTaken = true;
                        }
                    }
                    else if (playerDist.magnitude >= 1.5f) speed = 50;
                    if (playerDist.magnitude > rango)//Change to SEARCHING
                    {
                        actualState = enemyState.SEARCHING;
                        lastState = enemyState.FIGHTING;
                        speed = 25;
                        //playerAnim.SetBool("Is_Detected", true);
                        lastSeenPosition = GameObject.Find("Jugador").transform.position;
                        KarlinusEspectre.SetActive(true);
                        KarlinusEspectre.transform.position = lastSeenPosition;
                        playerAnim.SetBool("Is_Detected", true);
                        playerAnim.ResetTrigger("Is_Withdrawing");
                        playerAnim.SetTrigger("Is_Withdrawing");
                    }
                }
                else if (playerAnim.GetBool("Is_Damaging") && GetComponent<Collider>().enabled == false) speed = 0;
                break;
            default:
                break;
        }

        Start();
        discovered = false;        
        vecEnemy1.Normalize();
        if (dead) speed = 0;
    }

    public bool canBeKilled()
    {
        if (actualState == enemyState.FIGHTING && canAtackRef + 2.0f > Time.realtimeSinceStartup)
            return true;
        else if (actualState != enemyState.FIGHTING) return true;
        return false;
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        playerAnim.SetTrigger("Is_Sheathing");
        playerAnim.SetBool("Is_Detected", false);
        playerAnim.ResetTrigger("Is_Hitting");
        playerMovement.state = movement.playerState.IDLE;
        transform.gameObject.SetActive(false);
    }

    IEnumerator playerDeath(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene("DEAD");
    }
}


