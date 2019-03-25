using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    private Image image;

    public Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        image = GameObject.FindWithTag("MainImage").gameObject.GetComponent<Image>();
        image.sprite = sprite;
    }

}
