using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string originScene;

    [HideInInspector] public int ID = 0;
    public int currentSceneNumber = 0;

    //public Dictionary<SpriteName, Sprite> SpriteDictionary = null;
    [HideInInspector] public Dictionary<string, string> Emotion = null;

    public List<Recipe> recipes = new List<Recipe>();
    public FoodName cookedFood = FoodName.trash;

    public List<CharacterName> impressedPeople = new List<CharacterName>();

    public IManager currentSceneManager;
    public Language language;

    public string textPath
    {
        get
        {
            if (language == Language.Korean) return "Text/";
            else return "Text/" + language.ToString() + "/";
        }
    }

    private void Awake()
    {
        originScene = SceneManager.GetActiveScene().name;
        #region singleton
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Title")
            {
                Destroy(instance.gameObject);
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        #endregion

        #region emotionAssignment
        if(Emotion == null)
        {
            Emotion = new Dictionary<string, string>();
            Emotion.Add("걱정스러운", "worried");
            Emotion.Add("겁먹은", "scared");
            Emotion.Add("의아한", "wondering");
            Emotion.Add("놀란", "surprised");
            Emotion.Add("웃으며", "smiling");
            Emotion.Add("당황한", "dangwhang");
            Emotion.Add("풀죽은", "low");
            Emotion.Add("생각하는", "thoughtful");
            Emotion.Add("하품", "yawning");
            Emotion.Add("피곤한", "tired");
            Emotion.Add("고민하는", "pondering");
            Emotion.Add("머쓱한", "embarassed");
            Emotion.Add("생각난 듯", "reminded");
            Emotion.Add("유심히", "carefully");
            Emotion.Add("굳은", "grim");
            Emotion.Add("험악한", "rough");
            Emotion.Add("찡그린", "frowning");
            Emotion.Add("쓸쓸한", "lonely");
            Emotion.Add("구토", "vomit");
            Emotion.Add("민망한", "embarassed");
            Emotion.Add("한숨", "sigh");
            Emotion.Add("개운한", "refreshed");
            Emotion.Add("기운 없는", "limp");
            Emotion.Add("자조하며", "selfScorn");
            Emotion.Add("짜증난", "annoyed");
            Emotion.Add("한풀 꺾인", "relieved");
            Emotion.Add("빨개진", "blushed");
            Emotion.Add("아픈", "sick");
            Emotion.Add("아픈 씁쓸하게", "sickBitter");
            Emotion.Add("아픈 미소", "sickSmile");
            Emotion.Add("아픈 의아한", "sickWondering");
            Emotion.Add("아픈 의미심장한", "sickMeaningful");
            Emotion.Add("망설이며", "hesitating");
            Emotion.Add("가라앉은", "sunken");
            Emotion.Add("슬픈 미소", "sadSmile");
            Emotion.Add("분노한", "angry");
            Emotion.Add("결심한", "determined");
            Emotion.Add("슬픈", "sad");
        }
        #endregion

        #region recipe
        string[] rawRecipe = Resources.Load<TextAsset>("Text/Recipe").text.Split(new[] {"\r\n", "\n" }, StringSplitOptions.None);
        for(int i = 0; i < rawRecipe.Length; i++)
        {
            recipes.Add(new Recipe(rawRecipe[i].Split('\t')));
        }
        #endregion

        //    if (SpriteDictionary == null)
        //    {
        //        SpriteDictionary = new Dictionary<SpriteName, Sprite>();
        //        foreach (string name in System.Enum.GetNames(typeof(SpriteName)))
        //        {
        //            SpriteName sn = (SpriteName)System.Enum.Parse(typeof(SpriteName), name);
        //            Sprite sp = Resources.Load<Sprite>("Image/Character/" + name);
        //            SpriteDictionary.Add(sn, sp);
        //        }
        //    }

        if(PlayerPrefs.HasKey("language"))
        {
            language = (Language) Enum.Parse(typeof(Language), PlayerPrefs.GetString("language"));
        }

        if (instance == this && (SceneManager.GetActiveScene().name == "Dialogue"))
        {
            Proceed();
        }
    }
    public IEnumerator DelayedSceneChange(string name)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(name);
    }
    public IEnumerator DelayedSceneChange(GameSceneType name)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(name.ToString());
    }
    //private void Update()
    //{
    //    if(!sceneNumberIncrement)
    //        Debug.Log(instance.sceneNumberIncrement);
    //}
    [SerializeField] private bool sceneNumberIncrement = false;
    private Queue<GameSceneType> sceneQueue = new Queue<GameSceneType>();
    public List<string> rawDialogue;
    public void Proceed()
    {
        //Debug.Log("proceed" + sceneQueue.Count);
        if(sceneQueue.Count == 0)
        {
            if (instance.sceneNumberIncrement)
            {
                Debug.Log("next scene setup");
                Debug.Log(currentSceneNumber);
                currentSceneNumber++;
                Debug.Log(currentSceneNumber);
                sceneNumberIncrement = false;
            }
            Saver.SaveFile(this, 0);
            //Debug.Log(currentSceneNumber);
            
            var ta_kor = Resources.Load<TextAsset>(textPath + currentSceneNumber.ToString());
            if(ta_kor == null)
            {
                StartCoroutine(DelayedSceneChange(GameSceneType.Ending));
                return;
            }
            else
            {
                rawDialogue.Clear();
                var raw = ta_kor.text;
                string[] rows = raw.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                GameSceneType lst = GameSceneType.Clean;
                for (int i = 0; i < rows.Length; i++)
                {
                    if (Enum.TryParse(rows[i], out GameSceneType result))
                    {
                        sceneQueue.Enqueue(result);
                        lst = result;
                        continue;
                    }

                    switch (lst)
                    {
                        case GameSceneType.Dialogue:
                            rawDialogue.Add(rows[i]);
                            //Debug.Log(rows[i]);
                            break;
                    }
                }
            }
           
        }
        var s = sceneQueue.Dequeue();
        Debug.Log(s);
        if (sceneQueue.Count == 0) sceneNumberIncrement = true;
        instance.StartCoroutine(instance.DelayedSceneChange(s));
    }


    public void ToTitle()
    {
        Debug.Log("to title");
        instance.StartCoroutine(instance.DelayedSceneChange("Title"));
    }

    public void ChoiceResult(string result)
    {
        rawDialogue.Clear();
        var t = Resources.Load<TextAsset>(textPath + result).text;
        rawDialogue.AddRange(t.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None));
        StartCoroutine(DelayedSceneChange(GameSceneType.Dialogue));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public static void SetLanguage(Language lang)
    {
        instance.language = lang;
        instance.currentSceneManager.ChangeLanguage(lang);
        Saver.SetLanguage(lang);
    }
}

public struct Recipe
{
    public FoodName result;
    private Dictionary<Language, string> Names;
    public string name { get { return Names[GameManager.instance.language]; } }
    public CookProcess process;
    public Dictionary<IngredientType1, IngredientName> ingredients;
    public bool strong;

    public Recipe(string[] row)
    {
        Names = new Dictionary<Language, string>();
        ingredients = new Dictionary<IngredientType1, IngredientName>();
        strong = false;
        if (row.Length > 0)
        {
            result = (FoodName)Enum.Parse(typeof(FoodName), row[0]);

            Names[Language.Korean] = row[1];
            if (row.Length > 7) Names[Language.English] = row[7];
            else
            {
                Names[Language.English] = result.ToString();
            }

            process = (CookProcess)Enum.Parse(typeof(CookProcess), row[2]);

            for (int i = 3; i < 6; i++)
            {
                ingredients[(IngredientType1)i - 3] = (IngredientName)Enum.Parse(typeof(IngredientName), row[i]);
            }
            strong = (row[6] == "strong");
        }
        else
        {
            Names[Language.English] = "trash";
            Names[Language.Korean] = "음식물 쓰레기";
            result = FoodName.trash;
            process = CookProcess.none;
        }
    }
}