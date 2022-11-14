using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), ExecuteInEditMode]
public class Atk_Parabola : Atk_Range
{
    private Mesh mesh;
    [SerializeField] private Transform endPoint;

    [SerializeField] private float meshWidth;

    [SerializeField] private int resolution = 10;
    [SerializeField] public float maxYpos = 3;
    [HideInInspector] public float distance = 3;
    [HideInInspector] public float velocity;
    [HideInInspector] public float angle;


    private float gravity; // force of gravity on the y axis
    [HideInInspector] public float radianAngle;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        gravity = Mathf.Abs(Physics.gravity.y);
        MakeArcMesh(CalculateArcArray());
        this.gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        if (mesh != null)
            MakeArcMesh(CalculateArcArray());
    }

    private void Start()
    {
        MakeArcMesh(CalculateArcArray());
    }

    void MakeArcMesh(Vector3[] arcVerts)
    {
        if (mesh != null)
            mesh.Clear();
        else
        {
            mesh = GetComponent<MeshFilter>().mesh;
            gravity = Mathf.Abs(Physics.gravity.y);
            mesh.Clear();
        }

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

        // Use Angle
        // endPoint.localPosition = vertices[((resolution + 1) * 2) - 1] + new Vector3(meshWidth * 0.5f, 1.75f, 0);
        // Use LookAt
        endPoint.localPosition = vertices[1] + new Vector3(meshWidth * 0.5f, 0.01f, 0);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];

        //CalculateAngleWithMaxDistance();

        for (int i = 0; i <= resolution; ++i)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, distance);
        }

        return arcArray;
    }

    void CalculateAngleWithMaxDistance()
    {
        float middle = distance * 0.5f;

        for (float i = 1; i < 90; i += 0.5f)
        {
            angle = i;
            radianAngle = Mathf.Deg2Rad * angle;
            velocity = Mathf.Sqrt((distance * gravity) / Mathf.Sin(2 * radianAngle));
            float y = middle * Mathf.Tan(radianAngle) - ((gravity * middle * middle) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));

            if (y >= maxYpos)
                break;
        }
    }

    Vector3 CalculateArcPoint(float t, float dist)
    {
        //float y = x * Mathf.Tan(radianAngle) - ((gravity * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));

        float x = t * dist;
        float y = x * ((4 * maxYpos) / (maxYpos * dist)) * ((x / dist) - 1) * -maxYpos;

        return new Vector3(x, y);
    }

    public override void Calculate_Range(float maxdistance, Vector3 direction)
    {
        if (Mathf.Approximately(direction.magnitude, 0))
        {
            distance = 0.1f;
            return;
        }

        distance = maxdistance * direction.magnitude;
        MakeArcMesh(CalculateArcArray());


        transform.position = owner.position +
                                          direction.normalized * ((distance))
                                          + new Vector3(0, 0.001f, 0);

        transform.LookAt(owner);
    }
}