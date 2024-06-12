using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public void GameEnd(bool isWin)
    {
        GameManager.Instance.GameEnd(isWin);
        Debug.Log(isWin);
    }
}
