using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public int ID;
    //public Dictionary<SpriteName, Sprite> SpriteDictionary = null;
    public Dictionary<string, string> Emotion = null;


    private void Awake()
    {
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
    }
    public void Proceed()
    {

    }
}
