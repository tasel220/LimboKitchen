using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;

public class DialogueManager : MonoBehaviour, IManager
{
    private int currentIndex = -1; //1부터 세기
    List<Line> speeches = new List<Line>();
    private CharacterName charName;

    //public int SceneNumber;
    public Image background;
    public Image leftSpeaker;
    public Image rightSpeaker;
    public Text printName;
    public Text utterance;

    public GameObject choice2;
    public Text choiceText2;

    public float textDelay;


    public GameObject pointerL;
    public GameObject pointerR;


    private bool allowNext = true;


    private void Start()
    {
        GameManager.instance.currentSceneManager = this;
        //leftSpeaker.sprite = GameManager.instance.SpriteDictionary[SpriteName.None];
        //rightSpeaker.sprite = GameManager.instance.SpriteDictionary[SpriteName.None];
        leftSpeaker.sprite = GetSprite(CharacterName.None);
        rightSpeaker.sprite = GetSprite(CharacterName.None);
        //ParseDialogue(Resources.Load<TextAsset>("Text/Scene" + SceneNumber.ToString()).text);
        if (GameManager.instance != null)
        {
            ParseDialogue(GameManager.instance.rawDialogue);
        }
        NextTalk();
    }

    //public void ParseDialogue(string rawDialogue)
    public void ParseDialogue(List<string> rows)
    {
        speeches.Clear();

        string[] f = rows[0].Split('\t');

        background.sprite = Resources.Load<Sprite>("Image/Background/" + f[0]);

        if (f.Length > 1) charName = (CharacterName)Enum.Parse(typeof(CharacterName), f[1]);

        for(int i = 1; i < rows.Count; i++)
        {
            var lastLine = new Line(rows[i], charName);
            speeches.Add(lastLine);
            if(lastLine.lineType == LineType.enter && lastLine.spriteName != CharacterName.Dowoon)
            {
                charName = lastLine.spriteName;
            }
            else if(lastLine.lineType == LineType.reaction)
            {
                speeches.Remove(lastLine);
                Debug.Log(GameManager.instance.cookedFood);
                var reactionRows = Resources.Load<TextAsset>("Text/" + lastLine.reactionFile).text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                bool foodFound = false;
                for(int k = 0; k < reactionRows.Length; k++)
                {
                    var splitRow = reactionRows[k].Split(new[] { '\t', ':' });
                    var splitRowList = new List<string>(splitRow);
                    Debug.Log(splitRow[0]);
                    if(Enum.TryParse(splitRow[0], out FoodName fn))
                    {
                        if (foodFound) break;
                        if (reactionRows[k].Contains(GameManager.instance.cookedFood.ToString()))
                        foodFound = true;
                        if (!GameManager.instance.impressedPeople.Contains(charName))
                        {
                            GameManager.instance.impressedPeople.Add(charName);
                        }
                    }
                    else if(splitRow[0] == "else")
                    {
                        
                        if (foodFound) break;
                        foodFound = true;
                        if (GameManager.instance.impressedPeople.Contains(charName))
                        {
                            GameManager.instance.impressedPeople.Remove(charName);
                        }
                    }
                    else if(foodFound)
                    {
                        lastLine = new Line(reactionRows[k], charName);
                        speeches.Add(lastLine);
                        if (lastLine.lineType == LineType.enter && lastLine.spriteName != CharacterName.Dowoon)
                        {
                            charName = lastLine.spriteName;
                        }
                    }
                }
            }
        }
    }

    public void NextTalk()
    {
        if (!allowNext)
        {
            Debug.Log("아직"+currentIndex);
            return;
        }
        if (++currentIndex >= speeches.Count)
        {
            EndTalk();
        }

        else
        {
            //Debug.Log(currentIndex);
            var curLine = speeches[currentIndex];

            utterance.raycastTarget = false;
            choice2.SetActive(false);
            utterance.alignment = TextAnchor.UpperLeft;
            background.raycastTarget = true;
            //pointerL.SetActive(false);
            //pointerR.SetActive(false);

            switch (curLine.lineType)
            {
                case LineType.speech:
                    //Debug.Log(curLine.utterance);
                    if (curLine.right)
                        //rightSpeaker.sprite = GameManager.instance.SpriteDictionary[curSpeech.spriteName];
                        rightSpeaker.sprite = GetSprite(curLine.spriteName, curLine.emotion);
                    else
                    {
                        leftSpeaker.sprite = GetSprite(curLine.spriteName, curLine.emotion);
                    }
                    printName.text = curLine.printName;
                    utteranceToPrint = curLine.utterance;
                    StartCoroutine(Type());
                    //utterance.text = "";
                    //utterance.DOText(curSpeech.utterance, 0.5f);
                    break;

                case LineType.narration:

                    printName.text = curLine.printName;
                    utteranceToPrint = curLine.utterance;
                    StartCoroutine(Type());
                    //utterance.text = "";
                    //utterance.DOText(curSpeech.utterance, 0.5f);
                    break;
                case LineType.enter:
                    if (curLine.right)
                        //rightSpeaker.sprite = GameManager.instance.SpriteDictionary[curSpeech.spriteName];
                        rightSpeaker.sprite = GetSprite(curLine.spriteName, curLine.emotion);
                    else
                        leftSpeaker.sprite = GetSprite(curLine.spriteName, curLine.emotion);
                    //charName = curLine.spriteName;
                    NextTalk();
                    break;

                case LineType.exit:
                    //if (leftSpeaker.sprite.name.Contains(curLine.spriteName.ToString()))
                    leftSpeaker.sprite = GetSprite(CharacterName.None);
                    Debug.Log(charName + " 퇴장");
                    //else if (rightSpeaker.sprite.name.Contains(curLine.spriteName.ToString()))
                    //    rightSpeaker.sprite = GetSprite(CharacterName.None);

                    NextTalk();
                    break;

                case LineType.choice:
                    choice2.SetActive(true);
                    utterance.text = curLine.choice1;
                    choiceText2.text = curLine.choice2;
                    utterance.raycastTarget = true;
                    utterance.alignment = TextAnchor.UpperCenter;
                    background.raycastTarget = false;
                    break;
            }

        }
    }

    private string utteranceToPrint;
    private IEnumerator Type()
    {
        allowNext = false;
        utterance.text = "";
        for(int i = 0; i < utteranceToPrint.Length; i++)
        {
            utterance.text += utteranceToPrint[i];
            yield return new WaitForSeconds(textDelay);
        }
        allowNext = true;
    }
    
    public void EndTalk()
    {
        GameManager.instance.rawDialogue.Clear();
        GameManager.instance.Proceed();
    }

    struct Line
    {
        public Dictionary<Language, string> PrintName;
        public string printName
        {
            get { return PrintName[GameManager.instance.language]; }
        }
        public CharacterName spriteName;
        public string emotion;
        public Dictionary<Language, string> Utterance;
        public string utterance
        {
            get { return Utterance[GameManager.instance.language]; }
        }
        public bool right;

        public LineType lineType;

        public string choice1;
        public string choice2;

        public string[] result;

        public string reactionFile;

        public Line(string row, CharacterName charName, string row_eng = "")
        {
            char[] trimmers = { ':', '\t' };
            string[] s = row.Split(trimmers);
            right = false;
            spriteName = charName;
            emotion = "";
            Utterance = new Dictionary<Language, string>();
            PrintName = new Dictionary<Language, string>();
            choice1 = "";
            choice2 = "";
            result = new string[] { "", ""};
            reactionFile = "";

            s[0] = s[0].Trim();
            if (s[0].StartsWith("/"))
            {
                lineType = LineType.narration;
                Utterance[Language.Korean] = s[0];
                Utterance[Language.Korean] = Utterance[Language.Korean].Replace("/", "");
            }
            else if (s[0].Contains("enter"))
            {
                lineType = LineType.enter;

                if (s[0] != "enter")
                    s = s[0].Split(' ');
                spriteName = (CharacterName) Enum.Parse(typeof(CharacterName), s[1]);
                if (s.Length > 2) emotion = s[2];

                if (spriteName == CharacterName.Dowoon)
                {
                    right = true;
                }
            }
            else if(s[0].Contains("exit"))
            {
                if (s[0] != "exit")
                    s = s[0].Split(' ');
                lineType = LineType.exit;
            }

            else if(s[0] == "choice")
            {
                lineType = LineType.choice;
                choice1 = s[1].Trim();
                choice2 = s[2].Trim();
            }
            
            else if (s[0] == "result")
            {
                lineType = LineType.result;
                result[0] = s[1];
                result[1] = s[2];
            }

            else if (s[0] == "reaction")
            {
                lineType = LineType.reaction;
                reactionFile = s[1];
            }

            else
            {
                lineType = LineType.speech;
                PrintName[Language.Korean] = s[0].Trim();
                Utterance[Language.Korean] = s[1];
                string[] u = Utterance[Language.Korean].Split(')');
                if(u.Length > 1)
                {
                    Utterance[Language.Korean] = u[1];
                    emotion = u[0].Trim();
                    emotion = emotion.Remove(0,1);
                }


                if (PrintName[Language.Korean] == "도운" || PrintName[Language.Korean] == "주인공")
                {
                    right = true;
                    spriteName = CharacterName.Dowoon;
                }
            }
            Utterance[Language.Korean] = Utterance[Language.Korean].Trim();
        }
    }

    private Sprite GetSprite(CharacterName charName, string emotion = "")
    {
        if (emotion == "")
        {
            return Resources.Load<Sprite>("Image/Character/" + charName);
        }

        emotion = GameManager.instance.Emotion[emotion];
        Sprite s = Resources.Load<Sprite>("Image/Character/" + charName + "_" + emotion);
        if(s == null) return Resources.Load<Sprite>("Image/Character/" + charName);
        return s;
    }

    private int choiceN;
    public void Choice(int num)
    {
        var nextLine = speeches[currentIndex + 1];

        if(nextLine.lineType == LineType.result)
        {
            choiceN = num;
            GameManager.instance.ChoiceResult(nextLine.result[choiceN - 1]);
        }
        else
        {
            NextTalk();

        }
    }

    public void ChangeLanguage(Language selectedLangugae)
    {

    }
}
