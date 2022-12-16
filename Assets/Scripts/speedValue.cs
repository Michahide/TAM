using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class speedValue : MonoBehaviour
{
    [SerializeField] TMP_Text carSpeed;
    Car car;
    
    public void showSpeed(){
        carSpeed.text = "Speed: " + car.Speed.ToString();
    }
}
