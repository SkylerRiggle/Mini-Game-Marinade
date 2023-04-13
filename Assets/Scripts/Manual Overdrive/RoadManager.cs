using UnityEngine;

public class RoadManager : MonoBehaviour
{
    private RoadSpline[] roads = new RoadSpline[0];
    private int roadIndex;

    private void Awake() => roads = transform.GetComponentsInChildren<RoadSpline>(true);

    public RoadSpline AssignRoad()
    {
        roadIndex = Random.Range(0, roads.Length);
        roads[roadIndex].gameObject.SetActive(true);
        return roads[roadIndex];
    }

    public void RemoveRoad() => roads[roadIndex].gameObject.SetActive(false);
}
