using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookProcessButton : MonoBehaviour
{
    public CookProcess type;
    private string[] names = { "끓이기", "굽기/튀기기", "굽기", "생식" };

    private void Start()
    {
        int x = transform.GetSiblingIndex();
        type = (CookProcess) x;

        transform.GetChild(0).GetComponent<Text>().text = names[x];
    }
    public void Select()
    {
        CookManager.instance.SelectCookProcess(type);
        CookManager.instance.marker.transform.position = transform.position;
    }
}
