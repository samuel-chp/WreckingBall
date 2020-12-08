using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    // Possible colors (= same as blocks)  
    public Color[] colors;
    public float probEmpty = 0.3f;
    private int indexColorToSpawn; // Index of the color of Ball to spawn
    private GameObject itemAttached;

    //Prefab of the item to spawn
    [SerializeField] private GameObject spawnItem;

    void Start()
    { 
        colors = GameObject.Find("Planet Bottom").GetComponent<CircleSegmentManager>().segmentColors;
    }

    //Destroy all remaining feathers
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Follower")
        {
            if(itemAttached != null)
            {
                Destroy(itemAttached);
            }
        }
    }

    //Generate new feathers
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "Follower")
        {
            indexColorToSpawn = Random.Range(1, colors.Length);
            
            if(Random.Range(0.0f, 1.0f) > probEmpty)
            {
                spawnItem.GetComponent<SpriteRenderer>().color = colors[indexColorToSpawn];
                itemAttached = GameObject.Instantiate(spawnItem, transform.position, Quaternion.identity);
            }
        }
    }
}
