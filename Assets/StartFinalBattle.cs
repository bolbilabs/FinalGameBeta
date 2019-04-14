using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFinalBattle : MonoBehaviour
{
    public List<GameObject> tanny;

    public void StartFight ()
    {
        GameControl.finalBattle = true;
        GameControl.StartBattle(tanny);
    }
}
