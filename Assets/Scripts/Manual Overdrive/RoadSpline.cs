using UnityEngine;

public struct CatmullRomSegment
{
    public Vector2 a;
    public Vector2 b;
    public Vector2 c;
    public Vector2 d;
}

public class RoadSpline : MonoBehaviour
{
    [SerializeField] private Vector2[] splineNodes = new Vector2[4];
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
    }

    private void CacheSplineSegments()
    {
        // Create the segment array.
        segments = new CatmullRomSegment[splineNodes.Length];

        // Compute for each spline control point.
        for (int index = 0; index < splineNodes.Length; index++)
        {
            // Create a new spline segment.
            CatmullRomSegment newSegment = new CatmullRomSegment();

            // Get the relevant control points.
            Vector2 P0 = CircularIndexing(index, -1);
            Vector2 P1 = splineNodes[index];
            Vector2 P2 = CircularIndexing(index, 1);
            Vector2 P3 = CircularIndexing(index, 2);

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
        return SplineToWorldSpace(
            segment.a + (segment.b * step) + (segment.c * step2) + (segment.d * step3)
        );
    }

    private Vector2 CircularIndexing(int index, int offset)
    {
        if (offset >= 0)
        {
            return splineNodes[(index + offset) % splineNodes.Length];
        }

        return splineNodes[index];
    }

    private void OnValidate() => CacheSplineSegments();

    private void OnDrawGizmosSelected()
    {
        // Draw the influencing nodes.
        Gizmos.color = nodeColor;
        foreach (Vector2 node in splineNodes)
        {
            Gizmos.DrawSphere(SplineToWorldSpace(node), nodeRadius);
        }

        // Draw a preview of the spline.
        Gizmos.color = traceColor;
        float step = traceStep;
        Vector3 pointA = SplineToWorldSpace(splineNodes[0]);
        Vector3 pointB;
        while (step <= splineNodes.Length)
        {
            pointB = SampleSpline(step);
            Gizmos.DrawLine(pointA, pointB);
            pointA = pointB;
            step += traceStep;
        }
    }

    private Vector3 SplineToWorldSpace(Vector2 splinePoint)
    {
        return transform.position + new Vector3(
            splinePoint.x,
            0,
            splinePoint.y
        );
    }
}