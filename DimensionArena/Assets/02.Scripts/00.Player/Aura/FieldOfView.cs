using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : Atk_Range
{
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    [Range(0,360)]
    public float viewAngle;
    public float ViewAngle { get { return viewAngle; } }
    [SerializeField]
    private float viewRadius;
    public float ViewRadius { get { return viewRadius; } }

    public float meshResolution;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;
    MeshCollider meshCollider;

    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        
    }

    public Vector3 DirFromAngle(float angleInDegrees,bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += owner.transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = 0; i <= stepCount; ++i)
        {
            float angle = owner.transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            
            viewPoints.Add(newViewCast.point);

        }
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uvs = new Vector2[vertexCount];

        int[] triangles = new int[(vertexCount - 2) * 3];

        uvs[0] = new Vector2(0.5f, 0f);
        vertices[0] = Vector3.zero;


        for (int i = 0; i < vertexCount - 1; ++i)
        {
            vertices[i + 1] = owner.transform.InverseTransformPoint(viewPoints[i]);
            uvs[i + 1] = new Vector2((float)i / (float)(vertexCount - 2), 1f);

            if (i < vertexCount -2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.uv = uvs;

        viewMesh.RecalculateNormals();
    }
    float angle360(Vector3 from, Vector3 to, Vector3 right)
    {
        float angle = Vector3.Angle(from, to);
        return (Vector3.Angle(right, to) > 90f) ? 360f - angle : angle;
    }
    void DrawFieldOfView(Vector3 direction)
    {
        float newAngle = angle360(this.transform.forward, direction,this.transform.right);
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = 0; i <= stepCount; ++i)
        {
            float angle = owner.transform.eulerAngles.y + newAngle - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            viewPoints.Add(newViewCast.point);

        }
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uvs = new Vector2[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        uvs[0] = new Vector2(0.5f, 0f);

        for (int i = 0; i < vertexCount - 1; ++i)
        {
            uvs[i + 1] = new Vector2((float)i / (float)(vertexCount - 2), 1f);
            vertices[i + 1] = owner.transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.uv = uvs;

        viewMesh.RecalculateNormals();
    }


    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle,true);
        RaycastHit hit;

        Physics.Raycast(owner.transform.position, dir, out hit, viewRadius);

        return new ViewCastInfo(false, owner.transform.position + dir * viewRadius, viewRadius, globalAngle);
    }

    public override void Calculate_Range(float maxdistance, Vector3 direction)
    {
        //DrawFieldOfView();
        DrawFieldOfView(direction);
    }
}
