using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSprite : MonoBehaviour
{
    SpriteRenderer sprite;
    float timer = 0.2f;

    public Color color;

    GameObject reticle;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        //transform.position = ReticleControl.Instance().transform.position;
        //transform.localScale = ReticleControl.Instance().transform.localScale;

        sprite.sprite = ReticleControl.Instance().GetComponent<SpriteRenderer>().sprite;
        sprite.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            Destroy(gameObject);
        }
    }
}
