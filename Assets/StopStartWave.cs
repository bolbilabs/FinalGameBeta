using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopStartWave : MonoBehaviour
{
    public bool isStart = false;

    public GameObject waver;

    // Start is called before the first frame update
    void Start()
    {
        if (!isStart)
        {
            waver.GetComponent<SpriteRenderer>().material.SetFloat("_WaveForce", 0.0f);
        }
        else
        {
            waver.GetComponent<SpriteRenderer>().material.SetFloat("_WaveForce", 10.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
