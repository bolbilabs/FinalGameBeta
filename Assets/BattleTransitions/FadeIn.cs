using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    private Material renderMat;

    public SimpleBlit simpleBlit;

    private float someValue = 0f;

    public float adjustFactor = 0.005f;

    public float delay = 0.1f;

    public bool effectFinished = false;


    // Start is called before the first frame update
    void Start()
    {
        renderMat = simpleBlit.TransitionMaterial;
        renderMat.SetFloat("_Fade", 1.0f);
        renderMat.SetFloat("_Cutoff", 1.0f);
        StopAllCoroutines();
        StartCoroutine(FadeEffect());
    }


    IEnumerator FadeEffect()
    {
        yield return new WaitForSeconds((0.6f));
        for (float i = 1.0f; i >= 0-adjustFactor; i -= adjustFactor)
        {
            renderMat.SetFloat("_Fade", i);
            yield return new WaitForSeconds((delay));
        }
        effectFinished = true;
    }
}
