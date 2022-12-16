using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject sand;
    [SerializeField] GameObject road;
    [SerializeField] int extent = 5;
    [SerializeField] int frontDistance = 10;
    //seberapa jauh player bisa ke belakang
    [SerializeField] int backDistance = -5;
    [SerializeField] int maxSomeTerrainRepeat = 3;

    Dictionary<int, TerrainBlock> map = new Dictionary<int, TerrainBlock>(50);
    TMP_Text gameOverText;

    TextMeshProUGUI playAgainButton;
    TextMeshProUGUI mainMenuButton;
    TextMeshProUGUI quitButton;

    private int playerLastMaxTravel;

    private void Start() {
        //setup gameover panel
        gameOverPanel.SetActive(false);
        gameOverText = gameOverPanel.GetComponentInChildren<TMP_Text>();
        playAgainButton = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
        mainMenuButton = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
        quitButton = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();

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
        //cek apakah player masih hidup
        if(player.IsDie && gameOverPanel.activeInHierarchy == false)
            StartCoroutine(ShowGameOverPanel());
        
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

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(3);
        player.enabled = false;

        gameOverText.text = "Your Score: " + player.MaxTravel;
        gameOverPanel.SetActive(true);
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
