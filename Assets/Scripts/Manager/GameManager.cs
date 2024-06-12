using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    float start;


    #region ½Ì±ÛÅæ

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";

                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }


        objectPool = GetComponent<ObjectPool>();

        OnGameStartAction += OnGameStart;
        //OnGameEndAction

        player = GameObject.FindGameObjectWithTag("Player");
    }

    #endregion
    private GameObject player;
    public ObjectPool objectPool; // ¿ÀºêÁ§Æ® Ç®

    public Transform SpawnEnemyPos;

    public Action OnGameStartAction;
    //public Action OnGameEndAction;

    public GameObject WinUI;
    public GameObject WinText;
    public GameObject LoseUI;

    public bool isGameover = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockAndShowCursor();
        }
      
    }

    void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockAndShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnEnable()
    {
        ResetGame();
    }


    public void ResetGame()
    {

        start = Time.time;

        LockAndHideCursor();

        if(player != null && player.GetComponent<Player>() != null)
        {
            float hp = player.GetComponent<Player>().MaxHealth;
            if ((hp != 0))
            {
                player.GetComponent<Player>().Health = hp;
                player.SetActive(true);
                player.transform.position = Vector3.zero;

            }
        }
        else
        {
            var t = objectPool.GetActor(CType.Player);
            if (t != null)
                t.transform.position = Vector3.zero;



        }


        OnGameStartAction.Invoke();

        isGameover = false;
    }

    public void OnGameStart()
    {
        StartCoroutine(temp());
        WinUI.SetActive(false);
        LoseUI.SetActive(false);

      
    }

    IEnumerator temp()
    {
        yield return new WaitForSeconds(1f);

        var t = objectPool.GetActor(CType.witch);
        if (t != null)
            t.transform.position = SpawnEnemyPos.position;




        Debug.Log("Game Started!");
    }


    private void Start()
    {

    }

    public void GameEnd(bool isWin)
    {
        if (isWin)
        {
            //½Â¸® ui
            WinUI.SetActive(true);
            //½Â¸® »ç¿îµå
        }
        else
        {
            LoseUI.SetActive(true);

        }
        objectPool.Restart();

        float t = Time.time - start;

        WinText.GetComponent<TextMeshProUGUI>().text = $"clear time : {t} sec" ;


        //OnGameEndAction.Invoke();
        isGameover = true;
        Debug.Log("Game Ended!");
    }

   
}
