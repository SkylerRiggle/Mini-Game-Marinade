using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GameObject game = null;

    // Start is called before the first frame update
    void Start()
    {
        game.GetComponent<Wanted>().Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
