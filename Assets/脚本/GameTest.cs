using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTest : MonoBehaviour
{
    public GameObject player;
    public GameObject presszone;

    public RepairTest repairpoint;

    public Slider press;
    public Slider repair;
    
    public Collider2D presszone2;
    private float playerpos;
    private float presspos;  
    private float b = 0;
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        playerpos =  player.transform.position.y - player.transform.localScale.y/2 ;
        presspos = presszone.transform.position.y + presszone2.bounds.size.y/2;
        GameStart();
    }
    public void GameStart()
    {
        float distance = playerpos - presspos;
        Debug.Log(distance);
        press.value = distance;
    }
    public void StartRead()
    {
        repair.gameObject.SetActive(true);
    }
    public void ReadLine(float a, RepairTest target)
    {
        b += a;
        repair.value = b;
        if (repair.value == repair.maxValue)
        {
            repair.gameObject.SetActive(false);
            target.Dead();
        }
        
    }
    public void FinishRead()
    {
        repair.gameObject.SetActive(false);
        repair.value = 0;
        b = 0;
        
        
    }
}
