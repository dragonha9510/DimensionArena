using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Atk_Parabola : MonoBehaviour
{
    private Mesh mesh;
    [SerializeField] private Transform endPoint;

    [SerializeField] private float meshWidth;

    private float velocity;
    [SerializeField] private float distance = 3;
    [SerializeField] private float angle;
    [SerializeField] private int resolution = 10;


    private float gravity; // force of gravity on the y axis
    private float radianAngle;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        gravity = Mathf.Abs(Physics.gravity.y);
    }

    private void OnValidate()
    {
        if (mesh != null && Application.isPlaying)
            MakeArcMesh(CalculateArcArray());
    }

    private void Start()
    {
        MakeArcMesh(CalculateArcArray());
    }

    void MakeArcMesh(Vector3[] arcVerts)
    {
        mesh.Clear();
        Vector3[] vertices = new Vector3[(resolution + 1) * 2];
        int[] triangles = new int[resolution * 12];

        for (int i = 0; i <= resolution; ++i)
        {
            // set vertices
            vertices[i * 2] = new Vector3(meshWidth * 0.5f, arcVerts[i].y, arcVerts[i].x);
            vertices[(i * 2) + 1] = new Vector3(meshWidth * -0.5f, arcVerts[i].y, arcVerts[i].x);

            // set triangles
            if (i != resolution)
            {
                triangles[i * 12] = i * 2;
                triangles[i * 12 + 1] = triangles[i * 12 + 4] = (i + 1) * 2;
                triangles[i * 12 + 2] = triangles[i * 12 + 3] = i * 2 + 1;
                triangles[i * 12 + 5] = (i + 1) * 2 + 1;

                triangles[i * 12 + 6] = i * 2;
                triangles[i * 12 + 7] = triangles[i * 12 + 10] = (i + 1) * 2;
                triangles[i * 12 + 8] = triangles[i * 12 + 9] = i * 2 + 1;
                triangles[i * 12 + 11] = (i + 1) * 2 + 1;
            }
        }

        endPoint.localPosition = vertices[((resolution + 1) * 2) - 1] + new Vector3(meshWidth * 0.5f, 1.75f, 0);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;
        //float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / gravity;
        velocity = Mathf.Sqrt((distance * gravity) / Mathf.Sin(2 * radianAngle));

        for (int i = 0; i <= resolution; ++i)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, distance);
        }

        return arcArray;
    }

    Vector3 CalculateArcPoint(float t, float dist)
    {
        float x = t * dist;
        float y = x * Mathf.Tan(radianAngle) - ((gravity * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));

        return new Vector3(x, y);
    }
}