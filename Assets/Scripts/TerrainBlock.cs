using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBlock : MonoBehaviour
{
    [SerializeField] GameObject main;
    [SerializeField] GameObject repeat;

    private int extent;

    public int Extent { get => extent; }

    public void Build(int extent) {
        //ngambil extent nya yang private int extent = parameter extent
        this.extent = extent;

        //pembatas ruang gerak
        //looping 
        //i=-1 -> batas kiri
        //i=0 -> batas kanan
        for (int i = -1; i <= 1; i++)
        {
            if(i == 0)
                continue;
            
            var m = Instantiate(main);
            m.transform.SetParent(this.transform);
            m.transform.localPosition =  new Vector3((extent + 1) * i, 0, 0);
            // ada *= ngebuat warnanya dicampur, dalam kasus ini warnanya dibikin jadi gelap
            m.transform.GetComponentInChildren<Renderer>().material.color *= Color.grey;
        };

        //buat block utama
        main.transform.localScale = new Vector3(
            x: extent * 2 + 1,
            y: main.transform.localScale.y,
            z: main.transform.localScale.z
        );

        //pengkondisian jika repeat tidak ada (untuk grass)
        if(repeat == null)
            return;
            
        //looping untuk repeat/garis tengah di jalan
        for (int x = - (extent + 1); x <= extent + 1; x++)
        {
            if(x == 0)
                continue;

            //posisi z nya harus sesuai sama posisi terrain block
            var r = Instantiate(repeat);

            //bikin r jadi child nya road
            r.transform.SetParent(this.transform);

            //atur posisi biar posisi ikutin parent
            r.transform.localPosition =  new Vector3(x, 0, 0);   
        }
    }
}
