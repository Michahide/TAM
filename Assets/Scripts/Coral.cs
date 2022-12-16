using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coral : MonoBehaviour
{
    //static akan mmebuat variabel ini shared pada semua coral
    public static List<Vector3> AllPositions = new List<Vector3>();
    
    private void OnEnable() {
        AllPositions.Add(this.transform.position);
    }

    private void OnDisable() {
        AllPositions.Remove(this.transform.position);
    }
}
