using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusSpawner : MonoBehaviour
{
    [SerializeField] GameObject octopusPrefab;
    [SerializeField] int spawnZPos = 7;
    [SerializeField] Player player;
    [SerializeField] float timeOut = 5;

    [SerializeField] float timer = 0;
    int playerLastMaxTravel_x = 0;
    int playerLastMaxTravel_z = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        // SpawnOctopus();
    }

    private void Update() {
        // jika player ada kemajuan
        if((int) player.CurrentTravel_x != playerLastMaxTravel_x || (int) player.CurrentTravel != playerLastMaxTravel_z )
        {
            timer = 0;
            playerLastMaxTravel_x = player.CurrentTravel_x;
            playerLastMaxTravel_z = player.CurrentTravel;
            return;
        }
        
        // kalau gak maju jalankan timer
        if (timer < timeOut)
        {
            timer += Time.deltaTime;
            return;
        }

        if(player.isJumping()==false && player.IsDie == false)
            SpawnOctopus();
    }

    private void SpawnOctopus(){
        player.enabled = false;
        var position = new Vector3(player.transform.position.x-0.1f, 0.2f, player.CurrentTravel + spawnZPos);
        var rotation = Quaternion.Euler(3,0,0);
        var octopusObject = Instantiate(octopusPrefab, position, rotation);
        var octopus = octopusObject.GetComponent<Octopus>();
        octopus.SetupTarget(player);
    }

    // IEnumerator void (true){
    //     while (true)
    //     {
    //         yield return 
    //     }
    // }


}
