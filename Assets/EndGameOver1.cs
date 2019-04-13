using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameOver1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameControl.numDeaths++;
        GameControl.EndBattleDeath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
