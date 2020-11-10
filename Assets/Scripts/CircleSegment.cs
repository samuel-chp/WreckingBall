using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSegment : MonoBehaviour
{
    private PolygonCollider2D _polygonCollider;
    
    [SerializeField] private int numPoints;
    [SerializeField] private float offset;
    
    private float _innerRadius;
    private float _outerRadius;
    private float _startAngle;
    private float _endAngle;

    public void Initialize(float innerRadius, float outerRadius, float startAngle, float endAngle)
    {
        float angleOffset = Mathf.Asin(offset / innerRadius);
        _innerRadius = innerRadius + offset;
        _outerRadius = outerRadius - offset;
        _startAngle = startAngle+angleOffset;
        _endAngle = endAngle-angleOffset;
        
        _polygonCollider = GetComponent<PolygonCollider2D>();
        
        CreateCustomCollider();
    }

    void CreateCustomCollider()
    {
        Vector2[] points = new Vector2[2*(numPoints+1)];
        float unitAngle = (_endAngle - _startAngle) / numPoints;
        float angle = _startAngle;
        for(int i = 0; i <= numPoints; i++)
        {
            points[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _innerRadius;
            angle += unitAngle;
        }
        for(int i = numPoints+1; i <= 2*numPoints+1; i++)
        {
            angle -= unitAngle;
            points[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _outerRadius;
        }

        _polygonCollider.points = points;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            GetComponent<SpriteRenderer>().color = other.GetComponent<SpriteRenderer>().color;
            Destroy(other.gameObject);
        }
    }
}
