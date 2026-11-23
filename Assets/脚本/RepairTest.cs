using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTest : MonoBehaviour
{
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Dead()
    {
        player.Repair(this);
        //GameObject a = Instantiate(gameObject);
        Destroy(gameObject);
    }
}
