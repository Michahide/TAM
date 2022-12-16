using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slider : MonoBehaviour
{  
    TMP_Text gameOverText;
    Car car;

    public void Start(){
    }

    public void speedCar(float newSpeed){
        car.Speed = newSpeed;
    }
}
