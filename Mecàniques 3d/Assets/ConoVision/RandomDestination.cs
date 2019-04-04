using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RandomDestination : MonoBehaviour {

    public Vector3 RandomNavmeshLocation(GameObject enemy, float radius)
    {
        Vector2 randomDirection = Random.insideUnitCircle * radius;
        Vector3 sourcePos;
        sourcePos = new Vector3(randomDirection.x, enemy.transform.position.y, randomDirection.y);
        sourcePos += enemy.transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(sourcePos, out hit, radius, 1) && hit.position.y == enemy.transform.position.y)
            {
                finalPosition = hit.position;
            }
        return finalPosition;
    }

    public Vector3 RandomNavmeshLocationNPC(GameObject enemy, float radius)
    {
        Vector2 randomDirection = Random.insideUnitCircle * radius;
        Vector3 sourcePos;
        sourcePos = new Vector3(randomDirection.x, enemy.transform.position.y, randomDirection.y);
        sourcePos += new Vector3(enemy.transform.position.x, 0.0f, enemy.transform.position.z);
        return sourcePos;
    }
}