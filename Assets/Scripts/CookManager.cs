using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookManager : MonoBehaviour, IManager
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

    public Text combine;
    public Text clearSelection;
    public Text finish;

    private FoodName Result;

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
        GameManager.instance.currentSceneManager = this;
        ChangeLanguage(GameManager.instance.language);
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

    //public FoodName result = FoodName.trash;
    public Recipe resultRecipe = new Recipe(new string[] { });
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
            resultRecipe = r;
            
            break;
        }

        SetResultText();

        ResultPanel.SetActive(true);
        resultImage.sprite = Resources.Load<Sprite>("Image/Food/" + resultRecipe.result.ToString());
        
    }

    public void Confirm()
    {
        if (resultRecipe.result == FoodName.trash)
        {
            StartCoroutine(GameManager.instance.DelayedSceneChange(GameSceneType.Cook));
        }
        else
        {
            GameManager.instance.cookedFood = resultRecipe.result;
            GameManager.instance.Proceed();
        }
    }

    public void ChangeLanguage(Language selectedLanguage)
    {
        
        languageChange?.Invoke();
        switch(selectedLanguage)
        {
            case Language.Korean:
                combine.text = "조합";
                clearSelection.text = "재선택";
                finish.text = "완성!";
                break;
            case Language.English:
                combine.text = "Combine";
                clearSelection.text = "Clear Selection";
                finish.text = "Finish";
                break;
        }
        SetResultText();
    }
    private void SetResultText()
    {
        switch (GameManager.instance.language)
        {
            case Language.Korean:
                resultText.text = resultRecipe.name + " 완성!";
                break;
            case Language.English:
                resultText.text = "You made " + resultRecipe.name + "!";
                break;
        }
        if (resultRecipe.result == FoodName.trash)
            switch (GameManager.instance.language)
            {
                case Language.Korean:
                    resultText.text = "음식물 쓰레기 완성!";
                    break;
                case Language.English:
                    resultText.text = "You made trash!";
                    break;
            }

        if (resultRecipe.result != FoodName.trash)
        {
            resultButtonText.text = GameManager.instance.language == Language.Korean ? "확인" : "Confirm";
        }
        else
        {
            resultButtonText.text = GameManager.instance.language == Language.Korean ? "재시도" : "Retry";
        }
    }
    public event Action languageChange;
}
