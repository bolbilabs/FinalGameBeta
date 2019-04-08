using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SortAllObjects : MonoBehaviour
{
    List<GameObject> sortingList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0;  i < GameObject.FindWithTag("Players").transform.childCount; i++)
        {
            sortingList.Add(GameObject.FindWithTag("Players").transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < GameObject.FindWithTag("MapEnemies").transform.childCount; i++)
        {
            sortingList.Add(GameObject.FindWithTag("MapEnemies").transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < GameObject.FindWithTag("Interactables").transform.childCount; i++)
        {
            sortingList.Add(GameObject.FindWithTag("Interactables").transform.GetChild(i).gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        sortingList = sortingList.OrderByDescending(o => o.transform.position.y).ToList();

        for (int i = 0; i < sortingList.Count; i++)
        {
            sortingList[i].GetComponent<SpriteRenderer>().sortingOrder = i;
        }
    }
}
