using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEndingSound : MonoBehaviour
{
    public AudioSource theEnding;
    public AudioClip theSounding;

    // Start is called before the first frame update
    void Start()
    {
        theEnding.PlayOneShot(theSounding);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
