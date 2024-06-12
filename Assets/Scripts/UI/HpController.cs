using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpController : MonoBehaviour
{
    public Slider Slider;

    public void SetHpfloat(float value)
    {
        Slider.value = value; 
    }
}
