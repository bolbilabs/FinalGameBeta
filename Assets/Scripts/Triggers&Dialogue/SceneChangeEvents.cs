using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeEvents : MonoBehaviour
{
    public void ToBattle()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void ToOverworld()
    {
        SceneManager.LoadScene("Overworld");
    }
}
