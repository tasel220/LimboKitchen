using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookManager : MonoBehaviour
{
    public static CookManager instance;
    public float maxCookTime;
    private CookProcess selectedCookProcess = CookProcess.none;

    public Dictionary<IngredientType1, IngredientName[]> IngredientGroups = new Dictionary<IngredientType1, IngredientName[]>();
    public Dictionary<IngredientType1, IngredientName> SelectedIngredient = new Dictionary<IngredientType1, IngredientName>();

    public Transform[] ingredientNamePanels;

    public GameObject CookingPanel;
    
    public Image processImage;
    public Slider timeSlider;

    public GameObject ResultPanel;
    public Image resultImage;
    public Text resultText;
    public Text resultButtonText;

    public GameObject marker;



    private void Awake()
    {
        instance = this;

        IngredientGroups.Add(IngredientType1.meat, new IngredientName[] { IngredientName.chicken, IngredientName.pork, IngredientName.beef, IngredientName.egg });
        IngredientGroups.Add(IngredientType1.seafood, new IngredientName[] { IngredientName.pollack, IngredientName.salmon, IngredientName.shrimp, IngredientName.clam });
        IngredientGroups.Add(IngredientType1.vegetable, new IngredientName[] { IngredientName.carrot, IngredientName.onion, IngredientName.potato, IngredientName.tomato});

        SelectedIngredient.Add(IngredientType1.meat, IngredientName.none);
        SelectedIngredient.Add(IngredientType1.seafood, IngredientName.none);
        SelectedIngredient.Add(IngredientType1.vegetable, IngredientName.none);

        CookingPanel.SetActive(false);
        ResultPanel.SetActive(false);
        IngredientNameButton.instances.Clear();
    }


    private void Start()
    {
        GameManager.instance.cookedFood = FoodName.trash;
    }

    private float elapsedTime = 0;
    private bool cooking = false;
    // Update is called once per frame
    void Update()
    {
        if(cooking)
        {
            elapsedTime += Time.deltaTime;
            timeSlider.value = elapsedTime / maxCookTime;
            if (timeSlider.value >= 1) Finish();
        }
    }

    public void OpenIngredient(IngredientType1 ig)
    {
        ingredientNamePanels[(int)ig].SetAsLastSibling();
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
        if(SelectedIngredient[IngredientType1.meat] == IngredientName.none && SelectedIngredient[IngredientType1.seafood] == IngredientName.none && SelectedIngredient[IngredientType1.vegetable] == IngredientName.none)
        {
            return;
        }

        if(selectedCookProcess == CookProcess.raw)
        {
            Finish();
        }
        else
        {
            CookingPanel.SetActive(true);
            processImage.sprite = Resources.Load<Sprite>("Image/CookProcess/" + selectedCookProcess.ToString());
            cooking = true;
        }
    }

    public void Reselect()
    {
        IngredientNameButton.MarkDeselectAll();
        SelectedIngredient[IngredientType1.meat] = IngredientName.none;
        SelectedIngredient[IngredientType1.seafood] = IngredientName.none;
        SelectedIngredient[IngredientType1.vegetable] = IngredientName.none;
    }

    public FoodName result = FoodName.trash;
    public void Finish()
    {
        cooking = false;
        ResultPanel.SetActive(true);

        foreach(Recipe r in GameManager.instance.recipes)
        {
            if (r.process != selectedCookProcess)
            {
                continue;
            }
            if ((elapsedTime > maxCookTime / 2) != r.strong)
            {
                continue;
            }

            bool different = false;
            //for (int j = 0; j < r.ingredients.Count; j++)
            //{
            //    Debug.Log(r.ingredients[(IngredientType1)j]);
            //}
            for (int j = 0; j < r.ingredients.Count; j++)
            {
                if (r.ingredients[(IngredientType1)j] != IngredientName.any &&
                    r.ingredients[(IngredientType1)j] != SelectedIngredient[(IngredientType1)j])
                {
                    different = true;
                    break;
                }
            }

            if (different)
            {
                continue;
            }

            result = r.result;
            resultText.text = r.name + " 완성!";
            break;
        }

        ResultPanel.SetActive(true);
        resultImage.sprite = Resources.Load<Sprite>("Image/Food/" + result.ToString());
        if (result != FoodName.trash)
        {
            resultButtonText.text = "확인";
        }
        else
        {
            resultButtonText.text = "재시도";
        }
    }

    public void Confirm()
    {
        if (result == FoodName.trash)
        {
            StartCoroutine(GameManager.instance.DelayedSceneChange(GameSceneType.Cook));
        }
        else
        {
            GameManager.instance.cookedFood = result;
            GameManager.instance.Proceed();
        }
    }
}
