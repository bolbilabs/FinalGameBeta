using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EnableChoice : MonoBehaviour
{

    public string choice1Text;
    public string choice2Text;
    [Space(10)]
    public string choice1Scene;
    public string choice2Scene;

    private GameObject choice1Image;
    private GameObject choice2Image;


    private void Awake()
    {
        choice1Image = GameObject.FindGameObjectWithTag("Choice1");
        choice2Image = GameObject.FindGameObjectWithTag("Choice2");
    }

    // Start is called before the first frame update
    void Start()
    {
        choice1Image.gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText(choice1Text);
        choice2Image.gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText(choice2Text);

        choice1Image.gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Button>().onClick.AddListener(Option1OnClick);
        choice2Image.gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Button>().onClick.AddListener(Option2OnClick);

        choice1Image.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        choice2Image.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }


    void Option1OnClick()
    {
        SceneManager.LoadScene(choice1Scene);
    }

    void Option2OnClick()
    {
        SceneManager.LoadScene(choice2Scene);
    }
}
