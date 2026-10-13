using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    [SerializeField] private float distance = 10;
    [SerializeField] private float angle = 30;
    [SerializeField] private float height = 1.0f;
    [SerializeField] private int segments = 10;
    [SerializeField] private Color color = Color.red;

    [SerializeField] private int scanFrequency = 30;
    [SerializeField] private LayerMask layers;

    Collider[] colliders = new Collider[50];
    Mesh mesh;
    int count;
    float scanInterval;
    float scanTimer;

    public void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }
    public void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    public void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);
    }

    Mesh CreateWedgeMEesh()
    {
        Mesh mesh = new Mesh();

        // Each segment has 4 triangles, top also has 2, bottom also has 2
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        int vert = 0;

        // Left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // Right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; i++)
        {
            // Calculate vertices for each segment - centres never change
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            // Far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            // Top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            // Bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft; // Opposite order so normals point outward from mesh

            currentAngle += deltaAngle;
        }



        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
            
        return mesh;
    }
    private void OnValidate()
    {
        mesh = CreateWedgeMEesh();
    }
    private void OnDrawGizmos()
    {
        // Draw gizmo of the FOV wedge
        if (mesh)
        {
            Gizmos.color = color;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        // Draw sphere around this game object
        Gizmos.DrawWireSphere(transform.position, distance);

        // Draw spheres where all detected collisions are
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.7f);
        }
    }
}
