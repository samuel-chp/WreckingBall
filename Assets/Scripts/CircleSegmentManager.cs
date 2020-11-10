using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSegmentManager : MonoBehaviour
{
    [SerializeField] private GameObject circleDelimiterPrefab;
    [SerializeField] private GameObject lineSegmentPrefab;
    [SerializeField] private GameObject circleSegmentPrefab;
    public GameObject planetCore;
    
    public int nLayer;
    public int nSlice;
    [SerializeField] private Color[] segmentColors;
    
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateCircleSegments();
        GenerateLineSegment();
        GenerateCircleDelimiter();
    }

    private void GenerateCircleDelimiter()
    {
        var radius = planetCore.transform.localScale.x / 2;
        var unitScale = (0.5 - radius) / nLayer;
        radius += (float)unitScale;
        for (int i = 0; i < nLayer; i++)
        {
            var circleDelimiter = Instantiate(circleDelimiterPrefab, transform);
            var renderer = circleDelimiter.GetComponent<CircleLineRenderer>();
            renderer.radius = radius;

            radius += (float)unitScale;
        }
    }

    private void GenerateLineSegment()
    {
        var unitAngle = 2*Math.PI / nSlice;
        var radius = GetComponent<SpriteRenderer>().bounds.size[0] / 2;
        for (int i = 0; i < nSlice; i++)
        {
            var angle = i * unitAngle + Math.PI/2;
            var line = Instantiate(lineSegmentPrefab, transform);
            var renderer = line.GetComponent<LineRenderer>();
            renderer.SetPosition(1, new Vector3(
                radius * Mathf.Cos((float)angle),
                radius * Mathf.Sin((float)angle),
                0f
                ));
        }
    }

    private void GenerateCircleSegments()
    {
        float unitAngle = (float) (Math.PI / nSlice);
        float unitScale = (1 - planetCore.transform.localScale.x) / nLayer;
        int order = nLayer*nSlice + 1;  // Careful : planetCore orderInLayer must be greater than this one
        for (int i = 0; i < nLayer; i++)
        {
            for (int j = 0; j < nSlice; j++)
            {
                GameObject segment = Instantiate(circleSegmentPrefab, transform);
                SpriteRenderer renderer = segment.GetComponent<SpriteRenderer>();
                Material segmentMaterial = renderer.material;
                
                // Size
                segment.transform.localScale =
                    planetCore.transform.localScale + (i + 1) * new Vector3(unitScale, unitScale, unitScale);

                // Color & layer
                renderer.sortingOrder = order;
                renderer.color = segmentColors[order % segmentColors.Length];

                // Shader for creating an arc
                segmentMaterial.SetFloat("_startAngle", 0f);
                segmentMaterial.SetFloat("_endAngle", unitAngle);

                // Setting the arc at the right position
                float rotate = -2 * j * unitAngle - (float) unitAngle;
                segment.transform.rotation = Quaternion.Euler(0f, 0f, rotate * Mathf.Rad2Deg);
                
                // Setting the collider
                CircleSegment circleSegment = segment.GetComponent<CircleSegment>();
                float angle = j*2*unitAngle;
                circleSegment.Initialize(
                    0.5f*(planetCore.transform.localScale.x + i * unitScale)/(segment.transform.localScale.x),
                    0.5f*(planetCore.transform.localScale.x + (i+1) * unitScale)/(segment.transform.localScale.x),
                    angle+rotate+Mathf.PI/2,
                    angle+rotate+Mathf.PI/2 + 2*unitAngle
                    );

                order -= 1;
            }
        }
    }
}
