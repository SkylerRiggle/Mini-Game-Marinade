// Title: RoadSpline
// Author: Skyler Riggle

using UnityEngine;

/// <summary>
/// A data structure used to save spline segment constants.
/// </summary>
public struct CatmullRomSegment
{
    public Vector2 a;
    public Vector2 b;
    public Vector2 c;
    public Vector2 d;
}

/// <summary>
/// This class handles the construction of both the mathematical 
/// representation of a catmull rom spline and a road mesh from 
/// this spline.
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RoadSpline : MonoBehaviour 
{
    [SerializeField] private float tension = 0.5f;
    [SerializeField] private float sampleStep = 0.1f;
    private CatmullRomSegment[] splineSegments;
    [SerializeField] private float roadRadius = 1;

    [Header("Gizmo Parameters:")]
    [SerializeField] private Vector2[] splineNodes = new Vector2[0];
    [SerializeField] private float nodeRadius = 0.25f;
    [SerializeField] private Color nodeColor = Color.red;
    [SerializeField] private Color traceColor = Color.green;

    private void Awake() 
    {
        CacheSplineConstants();
        GetComponent<MeshFilter>().mesh = GenerateRoadMesh();
    }

    /// <summary>
    /// Compute and store the line segment constants for the spline.
    /// </summary>
    private void CacheSplineConstants()
    {
        // Create an array to cache the segment constants.
        int numNodes = splineNodes.Length;
        splineSegments = new CatmullRomSegment[numNodes];

        for (int index = 0; index < numNodes; index++)
        {
            // Create a new segment structure for this segment.
            CatmullRomSegment segment = new CatmullRomSegment();

            // Get the segment's control points.
            Vector2 P0 = splineNodes[(index - 1 + numNodes) % numNodes];
            Vector2 P1 = splineNodes[index];
            Vector2 P2 = splineNodes[(index + 1) % numNodes];
            Vector2 P3 = splineNodes[(index + 2) % numNodes];

            // Compute the segment constants given its control points.
            segment.a = P1;
            segment.b = tension * (P2 - P0);
            segment.c = (tension * ((2 * P0) - P3)) + 
            ((tension - 3) * P1) + ((3 - (2 * tension)) * P2);
            segment.d = (tension * (P3 - P0)) + 
            ((2 - tension) * P1) + ((tension - 2) * P2);

            // Store the segment's constants.
            splineSegments[index] = segment;
        }
    }

    private Mesh GenerateRoadMesh()
    {
        // Create a new mesh object for the road.
        Mesh roadMesh = new Mesh();
        roadMesh.name = transform.name;

        // Generate the vertex, uv, and index arrays.
        int sampleCount = 2 * (Mathf.RoundToInt(splineNodes.Length / sampleStep) + 1);
        Vector3[] verticies = new Vector3[sampleCount];
        Vector2[] uvs = new Vector2[sampleCount];
        int[] indicies = new int[sampleCount * 3];

        float t = 0;
        int triangleIndex, currentV = 0;
        Vector3 samplePosition = Vector3.zero, sampleNormal = Vector3.zero;
        for (int vertIndex = 0; vertIndex < sampleCount; vertIndex++)
        {
            // Alternate between each triangle in a road quad mesh.
            if ((vertIndex % 2) == 0)
            {
                // Calculate the normal and sample position, then assign it to a vertex.
                samplePosition = SampleSpline(t);
                sampleNormal = SampleNormal(t) * roadRadius;
                verticies[vertIndex] = RelativeSpace(sampleNormal, samplePosition);

                uvs[vertIndex] = new Vector2(0, currentV);

                // Assign triangle indicies.
                triangleIndex = vertIndex * 3;
                indicies[triangleIndex] = vertIndex;
                indicies[triangleIndex + 1] = (vertIndex - 1 + sampleCount) % sampleCount;
                indicies[triangleIndex + 2] = (vertIndex - 2 + sampleCount) % sampleCount;
            }
            else
            {
                // Assign vertex position.
                verticies[vertIndex] = RelativeSpace(-sampleNormal, samplePosition);

                uvs[vertIndex] = new Vector2(1, currentV);

                t += sampleStep;
                currentV++;

                // Assign triangle indicies.
                triangleIndex = vertIndex * 3;
                indicies[triangleIndex] = vertIndex;
                indicies[triangleIndex + 1] = (vertIndex + 1) % sampleCount;
                indicies[triangleIndex + 2] = (vertIndex + 2) % sampleCount;
            }
        }

        roadMesh.SetVertices(verticies);
        roadMesh.SetTriangles(indicies, 0);

        roadMesh.RecalculateBounds();
        roadMesh.RecalculateNormals();
        roadMesh.SetUVs(0, uvs);

        return roadMesh;
    }

    private Vector3 SampleSpline(float t)
    {
        int segmentIndex = Mathf.FloorToInt(Mathf.Clamp(t, 0, splineSegments.Length));
        CatmullRomSegment segment = splineSegments[segmentIndex];
        t %= 1.0f;

        return RelativeSpace(
            segment.a +
            segment.b * t +
            segment.c * t * t +
            segment.d * t * t * t,
            transform.position
        );
    }

    private Vector2 SampleTangent(float t)
    {
        int segmentIndex = Mathf.FloorToInt(Mathf.Clamp(t, 0, splineSegments.Length));
        CatmullRomSegment segment = splineSegments[segmentIndex];
        t %= 1.0f;

        return (
            segment.b +
            segment.c * 2 * t +
            segment.d * 3 * t * t
        ).normalized;
    }

    private Vector2 SampleNormal(float t)
    {
        Vector2 tangent = SampleTangent(t);
        return new Vector2(-tangent.y, tangent.x);
    }

    private void OnValidate() => CacheSplineConstants();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = nodeColor;
        foreach (Vector2 node in splineNodes)
        {
            Gizmos.DrawSphere(RelativeSpace(node, transform.position), nodeRadius);
        }

        // Error checking to make sure a spline won't get stuck rendering.
        if (sampleStep == 0) return;

        // Draw a preview of the spline path.
        Gizmos.color = traceColor;
        float t = 0;
        Vector3 pointA = RelativeSpace(splineNodes[0], transform.position), pointB;
        while (t <= splineNodes.Length)
        {
            pointB = SampleSpline(t);
            Gizmos.DrawLine(pointA, pointB);
            pointA = pointB;
            t += sampleStep;
        }
    }

    private Vector3 RelativeSpace(Vector2 point, Vector3 origin) => origin + new Vector3(point.x, 0, point.y);
}