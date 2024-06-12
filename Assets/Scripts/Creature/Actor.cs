using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CType
{
    None = 0,
    Player = 1,
    witch = 2,
    Zombie = 3,
    FireBall = 4,
    Spark = 5
}

public class Actor : MonoBehaviour
{
    public CType ActorType;


   
}
