using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObject : MonoBehaviour
{
    public GameObject setActiveObject;
    // Start is called before the first frame update
    void Start()
    {
        setActiveObject.SetActive(true);
    }
}
