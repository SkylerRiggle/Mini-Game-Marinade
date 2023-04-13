using UnityEngine;

public class RoadManager : MonoBehaviour
{
    private GameObject[] roads = new GameObject[0];
    private int roadIndex = 0;

    private void Awake()
    {
        GameObject[] newRoads = new GameObject[transform.childCount];

        for (int index = 0; index < newRoads.Length; index++)
        {
            newRoads[index] = transform.GetChild(index).gameObject;
        }
    }

    public void AssignRoad()
    {
        roadIndex = Random.Range(0, roads.Length);
        roads[roadIndex].SetActive(true);
    }

    public void RemoveRoad() => roads[roadIndex].SetActive(false);
}
