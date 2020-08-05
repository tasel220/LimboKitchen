using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookProcessButton : MonoBehaviour
{
    public CookProcess type;
    private string[] names = { "볶기", "끓이기", "튀기기", "찌기", "굽기", "오븐" };

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
