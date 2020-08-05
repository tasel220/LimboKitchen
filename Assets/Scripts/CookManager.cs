using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookManager : MonoBehaviour
{
    public static CookManager instance;

    private CookProcess selectedCookProcess = CookProcess.none;

    public GameObject marker;


    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.anyKeyDown)
        //{
        //    GameManager.instance.Proceed();

        //}
    }

    public void OpenIngredient(IngredientType1 ig)
    {

    }

    public void SelectCookProcess(CookProcess cp)
    {
        selectedCookProcess = cp;
    }

    public void Combine()
    {
        if (selectedCookProcess == CookProcess.none)
        {
            return;
        }
    }
}
