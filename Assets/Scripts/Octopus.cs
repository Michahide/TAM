using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.z <= player.CurrentTravel - 20)
            return;
        
        transform.Translate(Vector3.back * Time.deltaTime * speed);

        if(this.transform.position.z <= player.CurrentTravel && player.gameObject.activeInHierarchy)
            // player.gameObject.SetActive(false);
            player.transform.SetParent(this.transform);
    }

    public void SetupTarget(Player target){
        this.player = target;
    }
}
