using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookProcessButton : MonoBehaviour
{
    public CookProcess type;
    private string[] names = { "끓이기", "굽기/튀기기", "굽기", "생식" };

    private Text text;
    int x;

    private void Awake()
    {
        text = transform.GetChild(0).GetComponent<Text>();
    }

    private void Start()
    {
        x = transform.GetSiblingIndex();
        type = (CookProcess) x;

        CookManager.instance.languageChange += SetName;
        SetName();
    }
    public void Select()
    {
        CookManager.instance.SelectCookProcess(type);
        CookManager.instance.marker.transform.position = transform.position;
    }

    private void SetName()
    {
        switch(GameManager.instance.language)
        {
            case Language.Korean:
                text.text = names[x];
                break;

            case Language.English:
                text.text = type.ToString();
                break;
        }
    }
}
