using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    private Vector2Int nextCollisionCoords;
    private GameObject nextCollision;

    void Start() {
        nextCollisionCoords = new Vector2Int(-1, -1);
        nextCollision = null;
    }

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


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().ThrowProjectile(this);
            Destroy(gameObject);
        }
    }
}
