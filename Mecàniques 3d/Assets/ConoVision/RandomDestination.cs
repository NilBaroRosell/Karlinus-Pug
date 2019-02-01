using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RandomDestination : MonoBehaviour {

    NavMeshAgent enemyAgent;

    // Use this for initialization
    void Awake () {
        enemyAgent = this.GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public static Vector3 findRandom(NavMeshAgent enemyAgent, GameObject enemy)
    {
        Vector3 destinationPoint = new Vector3();
        float randomX = Random.Range(0, 60);
        float randomZ = Random.Range(0, 60);
        // buscar un punt aleatori a una certa distància de l'actual i marcar-lo com a objectiu
        destinationPoint = new Vector3(enemy.transform.localPosition.x + randomX, enemy.transform.localPosition.y, enemy.transform.localPosition.z + randomZ);
        NavMeshPath path = new NavMeshPath();
        if (enemyAgent.CalculatePath(destinationPoint, path)) return destinationPoint;
        else destinationPoint = findRandom(enemyAgent, enemy);
        return destinationPoint;
    }
}