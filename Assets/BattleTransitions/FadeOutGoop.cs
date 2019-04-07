using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeOutGoop : MonoBehaviour
{
    private Material renderMat;

    public SimpleBlit simpleBlit;

    private float someValue = 0f;

    public float adjustFactor = 0.005f;

    public float delay = 0.1f;

    public bool effectFinished = false;

    public UnityEvent thisEvent; 

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.gameObject.GetComponent<FadeIn>().enabled = false;
        renderMat = simpleBlit.TransitionMaterial;
        renderMat.SetFloat("_Fade", 1.0f);
        StopAllCoroutines();
        StartCoroutine(FadeEffect());
    }


    IEnumerator FadeEffect()
    {

        for (float i = 0; i < 1 + adjustFactor; i += adjustFactor)
        {
            renderMat.SetFloat("_Fade", 1.0f);

            renderMat.SetFloat("_Cutoff", i);
            yield return new WaitForSeconds((delay));
        }
        effectFinished = true;
        thisEvent.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        //if (someValue <= 1 + adjustFactor)
        //{
        //    renderMat.SetFloat("_Cutoff", someValue);
        //    someValue += adjustFactor;
        //}
    }
}
