// Title: RoadSpline
// Author: Skyler Riggle

using UnityEngine;

public struct CatmullRomSegment
{
    public Vector3 a;
    public Vector3 b;
    public Vector3 c;
    public Vector3 d;
}

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RoadSpline : MonoBehaviour
{
    [SerializeField] private float roadWidth = 2;
    [SerializeField] private float roadStep = 0.1f;

    [Header("Spline Parameters:")]
    [SerializeField] private Vector3[] splineNodes = new Vector3[4];
    private CatmullRomSegment[] segments;

    [SerializeField] private float tension = 0.5f;

    [Header("Gizmos Parameters:")]
    [SerializeField] private float nodeRadius = 0.25f;
    [SerializeField] private float traceStep = 0.1f;

    [SerializeField] private Color nodeColor = Color.red;
    [SerializeField] private Color traceColor = Color.green;

    private void Awake()
    {
        CacheSplineSegments();
        GetComponent<MeshFilter>().mesh = ConstructRoadMesh();
    }

    private void CacheSplineSegments()
    {
        // Create the segment array.
        int size = splineNodes.Length;
        segments = new CatmullRomSegment[size];

        // Compute for each spline control point.
        for (int index = 0; index < size; index++)
        {
            // Create a new spline segment.
            CatmullRomSegment newSegment = new CatmullRomSegment();

            // Get the relevant control points.
            Vector3 P0 = splineNodes[(index + size - 1) % size];
            Vector3 P1 = splineNodes[index];
            Vector3 P2 = splineNodes[(index + 1) % size];
            Vector3 P3 = splineNodes[(index + 2) % size];

            // Calculate and store the segment vector constants.
            newSegment.a = P1;
            newSegment.b = tension * (P2 - P0);
            newSegment.c = 2 * tension * P0 + (tension - 3) * P1 + 
            (3 - 2 * tension) * P2 - tension * P3;
            newSegment.d = -tension * P0 + (2 - tension) * P1 + 
            (tension - 2) * P2 + tension * P3;

            // Store the segment into the array.
            segments[index] = newSegment;
        }
    }

    public Vector3 SampleSpline(float step)
    {
        // Ensure that the step value falls with the legal range.
        step = step % splineNodes.Length;

        // Extrapolate the index given the step value.
        int index = Mathf.FloorToInt(step);

        // Get the associated cached segment.
        CatmullRomSegment segment = segments[index];

        // Calculate the powers of t.
        step %= 1;
        float step2 = step * step;
        float step3 = step2 * step;

        // Return the resulting spline point in world space.
        return transform.position +
            segment.a + (segment.b * step) + (segment.c * step2) + (segment.d * step3);
    }

    private Mesh ConstructRoadMesh()
    {
        // Construct a new road mesh object.
        Mesh roadMesh = new Mesh();
        roadMesh.name = gameObject.name;

        // Create a vertex and index array for the new mesh.
        int numVerts = Mathf.FloorToInt(splineNodes.Length / roadStep) * 2;
        Vector3[] vertices = new Vector3[numVerts];
        int[] triangles = new int[3 * numVerts];

        // Populate vertex and index data.
        float step = 0;
        for (int index = 0; index < numVerts; index++)
        {
            if (index % 2 == 0)
            {
                vertices[index] = Vector3.forward * index + Vector3.right;
            }
            else
            {
                vertices[index] = Vector3.forward * (index - 1) + Vector3.left;
            }

            int triangleIndex = index * 3;
            triangles[triangleIndex] = index;
            triangles[triangleIndex + 1] = (index + 1) % numVerts;
            triangles[triangleIndex + 2] = (index + 2) % numVerts;

            step += roadStep;
        }

        // Assign the new mesh data.
        roadMesh.SetVertices(vertices);
        roadMesh.SetTriangles(triangles, 0);
        roadMesh.RecalculateBounds();
        roadMesh.RecalculateNormals();

        // Return the resulting mesh.
        return roadMesh;
    }

    private void OnValidate() => CacheSplineSegments();

    private void OnDrawGizmosSelected()
    {
        // Draw the influencing nodes.
        Gizmos.color = nodeColor;
        foreach (Vector3 node in splineNodes)
        {
            Gizmos.DrawSphere(transform.position + node, nodeRadius);
        }

        // Draw a preview of the spline.
        Gizmos.color = traceColor;
        float step = traceStep;
        Vector3 pointA = transform.position + splineNodes[0];
        Vector3 pointB;
        while (step <= splineNodes.Length)
        {
            pointB = SampleSpline(step);
            Gizmos.DrawLine(pointA, pointB);
            pointA = pointB;
            step += traceStep;
        }
    }
}