using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject sand;
    [SerializeField] GameObject road;
    [SerializeField] int extent = 5;
    [SerializeField] int frontDistance = 10;
    //seberapa jauh player bisa ke belakang
    [SerializeField] int backDistance = -5;
    [SerializeField] int maxSomeTerrainRepeat = 3;

    Dictionary<int, TerrainBlock> map = new Dictionary<int, TerrainBlock>(50);

    private int playerLastMaxTravel;

    private void Start() {
        //instantiate tanah di belakang
        for (int z = backDistance; z <= 0; z++)
        {
            CreateTerrain(sand,z);
        }
        
        //instantiate depan
        for (int z = 1; z <= frontDistance; z++)
        {   
            
            var prefab = GetNextRandomTerrainPrefab(z);

            //instantiate block
            CreateTerrain(prefab,z);
        }

        player.Setup(backDistance, extent);
    }

    private void Update(){
        //infinite terrain system
        if(player.MaxTravel == playerLastMaxTravel)
            return;
        
        playerLastMaxTravel = player.MaxTravel;

        //bikin terrain otomatis ke depan pas lagi jalan
        var randTBPrefab = GetNextRandomTerrainPrefab(player.MaxTravel + frontDistance);
        CreateTerrain(randTBPrefab, player.MaxTravel + frontDistance);

        //hapus terrain yang belakang pas lagi jalan
        var lastTB = map[player.MaxTravel-1 + backDistance];

        // TerrainBlock lastTB = map[player.MaxTravel + backDistance];
        // int lastPos = player.MaxTravel;
        // foreach (var (pos, tb) in map)
        // {
        //     if(pos < lastPos)
        //     {
        //         lastPos = pos;
        //         lastTB = tb;
        //     }
        // }

        //map isinya daftar semua scene terrainblock

        //hapus dari daftar
        map.Remove(player.MaxTravel - 1 + backDistance);
        //hilangkan terrain dari scene
        Destroy(lastTB.gameObject);
        //setup lagi supaya player gak bisa gerak ke belakang
        player.Setup(player.MaxTravel + backDistance, extent);

    }

    //method instantiate block
    private void CreateTerrain(GameObject prefab, int zPos){
                   
            var go = Instantiate(prefab, new Vector3(0, 0, zPos), Quaternion.identity);
            var tb = go.GetComponent<TerrainBlock>();   
            tb.Build(extent);
            
            map.Add(zPos, tb);
    }

    //method untuk mengatur agar tidak lebih dari n
    private GameObject GetNextRandomTerrainPrefab(int nextPos){
        bool isUniform = true;
        //posisi 1 ke belakang
        var tbRef = map[nextPos - 1];
        for (int distance = 2; distance <= maxSomeTerrainRepeat; distance++)
        {
            if (map[nextPos - distance].GetType() != tbRef.GetType())
            {
                isUniform = false;
                break;
            }
        }

        if(isUniform){
            if(tbRef is Sand)
                return road;
            else
                return sand;
        }

        //penentuan terrain block dengan probabilitas 50%
        return Random.value > 0.5f ? road : sand;
    }
}
