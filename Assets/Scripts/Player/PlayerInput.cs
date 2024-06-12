using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public string fireButtonName = "Fire1";    
    public KeyCode sparkButtonName = KeyCode.Q;    
    public string reloadButtonName = "Reload";

  


    public Vector2 move { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; } 
    public bool fire2 { get; private set; } 
    public bool reload { get; private set; }

    public Action<MagicType> OnFired;
    public Action OnMoved;


    private void Update()
    {
        if (GameManager.Instance != null
            && GameManager.Instance.isGameover)
        {
            move = Vector2.zero;
            rotate = 0;
            fire = false;
            fire2 = false;
            reload = false;
            return;
        }

        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(move.magnitude > 0)
        {
            OnMoved?.Invoke();
        }

        fire = Input.GetButtonDown(fireButtonName);
        fire2 = Input.GetKeyDown(sparkButtonName);

        if (fire)
        {
            OnFired?.Invoke(MagicType.FireBall);

        }

        if(fire2)
        {
            OnFired?.Invoke(MagicType.Spark);

        }

        // reload에 관한 입력 감지
        //reload = Input.GetButtonDown(reloadButtonName);
    }
}
