using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientTypeButton : MonoBehaviour
{
    private static List<IngredientTypeButton> instances = new List<IngredientTypeButton>();

    private Text text;
    public IngredientType1 type;

    
    private string[] names_type = { "육류", "어패류", "채소류" };
    private string[] names_type_eng = { "Meat", "Seafood", "Vegetable" };

    int x;
    private void Start()
    {
        x = transform.GetSiblingIndex();
        text = transform.GetChild(0).GetComponent<Text>();

        SetName();

        GetComponent<Button>().onClick.AddListener(Open);

        CookManager.instance.languageChange += LanguageChange;
    }

    public void Open()
    {
        CookManager.instance.OpenIngredient(type);
    }
    public void SetName()
    {
        type = (IngredientType1)x;
        switch(GameManager.instance.language)
        {
            case Language.Korean:
                text.text = names_type[x];
                break;
            case Language.English:
                text.text = names_type_eng[x];
                break;
        }
    }

    public void LanguageChange()
    {
        SetName();
    }
}
