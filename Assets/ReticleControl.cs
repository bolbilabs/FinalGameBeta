using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleControl : MonoBehaviour
{

    public GameObject ghost;

    private static ReticleControl instance;



    public static ReticleControl Instance()
    {
        return instance;
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    // Start is called before the first frame update
        void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GameObject Ghosty = Instantiate(ghost, transform.position, transform.rotation);
        Ghosty.transform.localScale = transform.localScale;

        //Color color = GetComponent<SpriteRenderer>().color;

        //color.a = 1;

        //Ghosty.GetComponent<SpriteRenderer>().color = color;
    }
}
