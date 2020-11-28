using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCoreGFX : MonoBehaviour
{
    public GameObject player;

    private SpriteRenderer _spriteRenderer;
    private Vector3 _gravityDirection;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _gravityDirection = player.transform.position - transform.position;
        
        // Angle & orientation
        float angle = Vector2.Angle(Vector2.up, _gravityDirection);
        if (player.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, angle-90);
            _spriteRenderer.flipX = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90-angle);
            _spriteRenderer.flipX = false;
        }
    }
}
