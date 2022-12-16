using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralSpawner : MonoBehaviour
{
    [SerializeField] GameObject coralPrefab;
    [SerializeField] TerrainBlock terrain;
    [SerializeField] int count = 3;
    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> emptyPos = new List<Vector3>();

        
        for (int x = - terrain.Extent; x <= terrain.Extent; x++)
        {
            /*pengkondisian agar ketika coral dispawn tidak akan 
            pernah spawn posisinya sama dengan hewan*/ 
            if(transform.position.z == 0 && x == 0)
                continue;
            
            emptyPos.Add(transform.position + Vector3.right * x);
        } 

        for (int i = 0; i < count; i++)
        {
            var index = Random.Range(0, emptyPos.Count);
            var spawnPos = emptyPos[index];
            Instantiate(
                coralPrefab,
                spawnPos,
                Quaternion.identity,
                this.transform
            );
            emptyPos.RemoveAt(index);
        }

        //Spawn coral di batas kiri
        Instantiate(
            coralPrefab,
            transform.position + Vector3.right * -(terrain.Extent+1),
            Quaternion.identity,
            this.transform
        );
        
        //Spawn coral di batas kiri
        Instantiate(
            coralPrefab,
            transform.position + Vector3.right * (terrain.Extent + 1),
            Quaternion.identity,
            this.transform
        );
    }
}
