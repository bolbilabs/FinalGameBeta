using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSongGo : MonoBehaviour
{
    public MonoBehaviour[] doThese;

    public int i = 0;

    private void Update()
    {
        if (i > 500)
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
