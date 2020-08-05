using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ingredient1Button : MonoBehaviour
{
    public IngredientType1 type;
    private string[] names = { "육류", "곡류", "가공식품", "어패류", "과채류", "조미료" };

    private void Start()
    {
        int x = transform.GetSiblingIndex();
        type = (IngredientType1)x;

        transform.GetChild(0).GetComponent<Text>().text = names[x];
    }
    public void Open()
    {
        CookManager.instance.OpenIngredient(type);
    }
}
