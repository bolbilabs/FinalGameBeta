using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSongGo : MonoBehaviour
{
    public MonoBehaviour[] doThese;

    public int i = 0;

    private void FixedUpdate()
    {
        if (i > 350)
        {
            if (doThese.Length > 0)
            {
                foreach (MonoBehaviour script in doThese)
                {
                    if (script != null)
                    {
                        script.enabled = true;
                    }
                }
            }
        }
        i++;
    }

}
