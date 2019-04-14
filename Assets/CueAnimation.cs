using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueAnimation : MonoBehaviour
{
    public string animationName;
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool(animationName, true);
    }

}
