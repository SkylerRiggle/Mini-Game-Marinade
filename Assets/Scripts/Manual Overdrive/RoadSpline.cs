using UnityEngine;

[System.Serializable]
public class SplineNode
{
    public Vector2 nodePosition = Vector2.zero;

    private Vector2 _weightVector = Vector2.zero;
    public Vector2 weightVector { get { return _weightVector; } }

    public void ComputeWeight(Vector2 startPosition, Vector2 endPosition, float scale)
    {
        _weightVector = (endPosition - startPosition) * scale;
    }
}

public class RoadSpline : MonoBehaviour
{
    [SerializeField] private SplineNode[] splineNodes = new SplineNode[0];
    [SerializeField] private float weightScale = 1;

    [SerializeField] private float nodeRadius = 0.25f;
    [SerializeField] private float traceStep = 0.1f;

    [SerializeField] private Color nodeColor = Color.red;
    [SerializeField] private Color weightColor = Color.green;
    [SerializeField] private Color traceColor = Color.white;

    private void Awake() 
    {
        ComputeSplineWeights();
    }

    private void ComputeSplineWeights()
    {
        SplineNode lastNode, currentNode, nextNode;
        int size = splineNodes.Length;
        for (int index = 0; index < size; index++)
        {
            lastNode = splineNodes[(index + size - 1) % size];
            currentNode = splineNodes[index];
            nextNode = splineNodes[(index + 1) % size];

            currentNode.ComputeWeight(lastNode.nodePosition, nextNode.nodePosition, weightScale);
        }
    }

    private void OnValidate() => ComputeSplineWeights();

    private void OnDrawGizmosSelected() 
    {
        float step, step3, beta, beta3;
        Vector3 drawOrigin, linePosition;
        SplineNode currentNode, nextNode;
        for (int index = 0; index < splineNodes.Length; index++)
        {
            // Get the current and next nodes.
            currentNode = splineNodes[index];
            nextNode = splineNodes[(index + 1) % splineNodes.Length];

            // Draw node origin.
            Gizmos.color = nodeColor;
            drawOrigin = RelativeSplinePosition(currentNode.nodePosition, transform.position);
            Gizmos.DrawSphere(drawOrigin, nodeRadius);

            // Draw node weights.
            Gizmos.color = weightColor;
            linePosition = RelativeSplinePosition(currentNode.weightVector, drawOrigin);
            Gizmos.DrawLine(drawOrigin, linePosition);

            // Draw the spline curvature preview.
            Gizmos.color = traceColor;
            step = 0;
            beta = 1;
            while (step < 1.0f)
            {
                step3 = Mathf.Pow(step, 3);
                beta3 = Mathf.Pow(beta, 3);

                linePosition = new Vector3(
                    0,
                    0,
                    0
                );
                Gizmos.DrawLine(drawOrigin, linePosition);

                drawOrigin = linePosition;
                step += traceStep;
                beta = 1 - step;
            }
        }
    }

    private Vector3 RelativeSplinePosition(Vector2 point, Vector3 origin)
    {
        return new Vector3(
            origin.x + point.x,
            origin.y,
            origin.z + point.y
        );
    }
}
