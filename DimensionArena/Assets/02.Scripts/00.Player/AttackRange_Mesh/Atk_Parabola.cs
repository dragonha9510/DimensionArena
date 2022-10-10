using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Atk_Parabola : MonoBehaviour
{
    private Mesh mesh;
    [SerializeField] private float meshWidth;

    [SerializeField] private float velocity;
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

    }

    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / gravity;

        for(int i = 0; i <= resolution; ++i)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;
    }

    Vector3 CalculateArcPoint(float t, float distance)
    {
        float x = t * distance;
        float y = x * Mathf.Tan(radianAngle) - ((gravity * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));

        return new Vector3(x, y);
    }
}
