using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{
    public GameObject setInactiveObject;
    // Start is called before the first frame update
    void Start()
    {
        setInactiveObject.SetActive(false);
    }
}
