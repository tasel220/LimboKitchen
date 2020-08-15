using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientNameButton : MonoBehaviour
{
    public static List<IngredientNameButton> instances = new List<IngredientNameButton>();

    public IngredientType1 type;
    public IngredientName ingredientName;

    private Image image;
    public Text text;
  
    private string[] names_meat = { "닭", "돼지", "소", "계란" };
    private string[] names_seafood = { "대구", "연어", "새우", "바지락" };
    private string[] names_vegetable = { "당근", "양파", "감자", "토마토"};

    private void Awake()
    {
        image = GetComponent<Image>();
        MarkDeselect();
        text = transform.GetChild(0).GetComponent<Text>();
    }
    int x;
    private void Start()
    {
        if (!instances.Contains(this))
            instances.Add(this);
        x = transform.GetSiblingIndex();
        ingredientName = CookManager.instance.IngredientGroups[type][x];

        SetName();

        CookManager.instance.languageChange += SetName;

        GetComponent<Button>().onClick.AddListener(() => SelectIngredient());


    }

    public void SelectIngredient()
    {
        if(CookManager.instance.SelectedIngredient[type] == ingredientName)
        {
            CookManager.instance.SelectedIngredient[type] = IngredientName.none;
            MarkDeselect();
        }
        else
        {
            for(int i = 0; i < instances.Count; i++)
            {
                if(instances[i].transform.parent == transform.parent)
                    instances[i].MarkDeselect();
            }
            CookManager.instance.SelectedIngredient[type] = ingredientName;
            MarkSelect();
        }
    }

    private void MarkSelect()
    {
        image.color = Color.white;

    }
    private void MarkDeselect()
    {
        image.color = Color.grey;
    }
    public static void MarkDeselectAll()
    {
        for (int i = 0; i < instances.Count; i++)
        {
            instances[i].MarkDeselect();
        }
    }

    private void SetName()
    {

        switch(GameManager.instance.language)
        {
            case Language.English:
                text.text = ingredientName.ToString();
                break;
            case Language.Korean:
                 switch (type)
                 {
                    case IngredientType1.meat:
                        text.text = names_meat[x];
                        break;
                    case IngredientType1.seafood:
                        text.text = names_seafood[x];
                        break;
                    case IngredientType1.vegetable:
                        text.text = names_vegetable[x];
                        break;
                 }
                break;
        }
    }
}
