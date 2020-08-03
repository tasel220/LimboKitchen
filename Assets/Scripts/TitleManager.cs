using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public void Continue()
    {

    }

    public void NewGame()
    {
        GameManager.instance.Proceed();
    }
}
