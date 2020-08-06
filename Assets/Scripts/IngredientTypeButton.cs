using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientTypeButton : MonoBehaviour
{
    private static List<IngredientTypeButton> instances = new List<IngredientTypeButton>();

    public IngredientType1 type;

    
    private string[] names_type = { "육류", "어패류", "채소류" };

    private void Start()
    {
        int x = transform.GetSiblingIndex();

        type = (IngredientType1)x;
        transform.GetChild(0).GetComponent<Text>().text = names_type[x];

        GetComponent<Button>().onClick.AddListener(Open);

    }

    public void Open()
    {
        CookManager.instance.OpenIngredient(type);
    }

    //private void MarkSelect()
    //{
    //    image.color = Color.white;

    //}
    //private void MarkDeselect()
    //{
    //    image.color = Color.green;
    //}
}
