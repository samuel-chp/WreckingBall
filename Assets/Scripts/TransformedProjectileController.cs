using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformedProjectileController : MonoBehaviour
{
    private Vector2Int nextCollisionCoords = new Vector2Int(-1, -1);
    private GameObject nextCollision = null;


    public void SetNextCollision(Vector2Int newCollisionCoords, GameObject newCollision){
        nextCollisionCoords = newCollisionCoords;
        nextCollision = newCollision;
    }

    public Vector2Int GetNextCollisionCoords() {
        return nextCollisionCoords ;
    }

    public GameObject GetNextCollision() {
        return nextCollision ;
    }
}
