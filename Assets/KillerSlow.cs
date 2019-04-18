using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerSlow : MonoBehaviour
{
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.pitch = ((float)(1.0f / (float) (5 - GameObject.FindGameObjectsWithTag("Player").Length)));
    }
}
