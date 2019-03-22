using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class kill_cono_vision : MonoBehaviour
{

    private struct targetData
    {
        public bool seen;
        public Vector3 killTargetPos;
        public csAreaVision targetState;
        public GameObject target;
        public Renderer targetRenderer;
        public Vector3 ghostPos;
    };

    targetData[] targets;
    private int targetI;
    private int errorAcum = 0;
    public static GameObject[] assignedTargets;
    static Animator anim;
    public static bool returnPlayer;
    private GameObject player;
    private liquidState liquidKill;
    private Controller playerMovement;
    private Vector3 playerPos;
    private bool aproaching;
    private bool stuck;
    private float stuckReference;
    enum killState { WATCHING, APROACHING, KILLING, RETURNING };
    public static string actualString;
    killState actualState;

    //Nav Mesh
    NavMeshAgent liquidAgent;


    // Use this for initialization
    void Awake()
    {
        targets = new targetData[2];
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = new targetData();
            targets[i].seen = false;
            targets[i].targetState = null;
            targets[i].target = null;
            targets[i].ghostPos = Vector3.zero;
            targets[i].killTargetPos = new Vector3(0.0f, 0.0f, 0.0f);
        }
        stuckReference = 0.0f;
        stuck = false;
        assignedTargets = new GameObject[2];
        player = GameObject.Find("Jugador");
        liquidKill = player.GetComponent<liquidState>();
        playerMovement = player.GetComponent<Controller>();
        liquidAgent = player.GetComponent<NavMeshAgent>();
        if (liquidAgent == null)
        {
            Debug.LogError("Nav Mesh error");
        }
        liquidAgent.enabled = false;
        actualState = killState.WATCHING;
        aproaching = false;
        anim = player.GetComponent<Animator>();
        returnPlayer = false;
    }

    void areaMesh()
    {
        if (assignedTargets[0] == assignedTargets[1]) assignedTargets[1] = null;
        targetI = 0;
        targets[0].seen = targets[1].seen = false;
        for (int i = 0; i < assignedTargets.Length; i++)
        {
            if (assignedTargets[i] != null && GameObject.Find("Jugador").GetComponent<liquidState>().hidratation > 0 && Input.GetKeyDown(KeyCode.Mouse0))
            {

                if (targets[targetI].seen) targetI = 1;
                targets[targetI].seen = true;
                targets[targetI].target = assignedTargets[i].transform.gameObject;
                targets[targetI].targetState = targets[targetI].target.GetComponent<csAreaVision>();
                playerPos = player.transform.position;
                playerMovement.state = Controller.playerState.HITTING;
                liquidAgent.enabled = true;
                targets[targetI].ghostPos = targets[targetI].target.transform.GetChild(4).gameObject.transform.position;
                targets[targetI].target.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                player.GetComponent<Collider>().enabled = false;
                targets[targetI].target.GetComponent<Collider>().enabled = false;
                targets[targetI].target.GetComponent<Rigidbody>().useGravity = false;
                anim.SetBool("Is_Damaging", true);
                targets[targetI].target.GetComponent<NavMeshAgent>().enabled = false;
                targets[targetI].target.GetComponent<NavMeshObstacle>().enabled = true;
                Destroy(targets[targetI].target.transform.GetChild(4).gameObject);
                stuckReference = Time.realtimeSinceStartup;
                liquidKill.firstFrameNormal = false;
                liquidKill.cooldown = false;
                liquidKill.showLiquid();
                actualState = killState.APROACHING;
                targets[targetI].killTargetPos = targets[targetI].target.transform.position;
            }
            else break;
        }
        if (targets[0].seen)
        {
            targetI = 0;
            liquidAgent.SetDestination(targets[targetI].ghostPos);
        }
    }

    // Update is called once per frame

    private void Update()
    {
        switch (actualState)
        {
            case killState.WATCHING:
                kill_vision();
                actualString = "W";
                break;
            case killState.APROACHING:
                if (!liquidAgent.isOnNavMesh && stuckReference + 2.5f > Time.realtimeSinceStartup)
                {
                    errorAcum++;
                    if (errorAcum > 10) aproachEnemy(targets[targetI].killTargetPos);
                    Vector3 destDirection = targets[targetI].ghostPos - player.transform.position;
                    destDirection.Normalize();
                    player.transform.position += destDirection;
                }
                else if (liquidAgent.remainingDistance <= 0.25f && Vector3.Magnitude(transform.position - liquidAgent.destination) >= 1.5f)
                {
                    Vector3 destDirection = targets[targetI].ghostPos - player.transform.position;
                    destDirection.Normalize();
                    targets[targetI].ghostPos = new Vector3(targets[targetI].ghostPos.x, transform.position.y, targets[targetI].ghostPos.z);
                    liquidAgent.ResetPath();
                    liquidAgent.SetDestination(targets[targetI].ghostPos);
                }
                else if (Vector3.Magnitude(transform.position - liquidAgent.destination) < 1.5f || stuckReference + 2.5f < Time.realtimeSinceStartup) aproachEnemy(targets[targetI].killTargetPos);
                else
                {
                    Vector3 destDirection = targets[targetI].ghostPos - player.transform.position;
                    destDirection.Normalize();
                    player.transform.position += destDirection / 10;
                }
                actualString = "A";
                break;
            case killState.KILLING:
                if (returnPlayer) setReturn();
                actualString = "K";
                break;
            case killState.RETURNING:
                if (!liquidAgent.isOnNavMesh && stuckReference + 2.5f > Time.realtimeSinceStartup)
                {
                    errorAcum++;
                    if (stuckReference + 1.0f < Time.realtimeSinceStartup || Vector3.Magnitude(transform.position - liquidAgent.destination) <= 0.25f) returnToPosition();
                    else
                    {
                        Vector3 destDirection = playerPos - player.transform.position;
                        destDirection.Normalize();
                        playerPos += destDirection * errorAcum;
                        player.transform.position = playerPos;
                        liquidAgent.SetDestination(playerPos);
                    }
                }
                else if ((liquidAgent.remainingDistance <= 0.25f && stuckReference + 0.5f < Time.realtimeSinceStartup) || stuckReference + 2.5f < Time.realtimeSinceStartup) returnToPosition();
                else errorAcum = 0;
                actualString = "R";
                break;
            default:
                break;
        }
    }
    public void kill_vision()
    {
        areaMesh();
        assignedTargets = new GameObject[2];
    }

    private void aproachEnemy(Vector3 destination)
    {
        errorAcum = 0;
        liquidKill.hideLiquid();
        player.transform.position = targets[targetI].ghostPos;
        player.transform.LookAt(destination);
        actualState = killState.KILLING;
        liquidAgent.enabled = false;
        aproaching = false;
        player.GetComponent<CharacterController>().SimpleMove(Vector3.zero);
        targets[targetI].target.gameObject.GetComponent<Animator>().SetTrigger("Is_Dying");
        targets[targetI].targetState.dead = true;
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
        errorAcum = 0;
        player.transform.position = playerPos;
        player.GetComponent<Collider>().enabled = true;
        liquidKill.setHidratation();
        liquidKill.hideLiquid();
        playerMovement.state = Controller.playerState.IDLE;
        liquidAgent.enabled = false;
        actualState = killState.WATCHING;
        assignedTargets = new GameObject[2];
    }


    void setReturn()
    {
        targetI++;
        anim.SetBool("Is_Damaging", false);
        stuckReference = Time.realtimeSinceStartup;
        liquidKill.showLiquid();
        liquidAgent.enabled = true;
        if (targetI < targets.Length && targets[targetI].seen)
        {
            actualState = killState.APROACHING;
            liquidAgent.SetDestination(targets[targetI].ghostPos);
        }
        else
        {
            actualState = killState.RETURNING;
            liquidAgent.SetDestination(playerPos);
        }
        returnPlayer = false;
    }
}
