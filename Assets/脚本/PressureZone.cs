using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class PressureZone : MonoBehaviour
{
    [Header("是否正在受压（只读）")]
    public bool isPressed;

    public float speed;

    public Light2D targetlight;

    public GameObject player;


    private bool isstoped = false;
    public void Start()
    {
        
    }
    public void Update()
    {
        if (isstoped||Time.time< 1.0f)
        {
            return;
        }
        else
        {
            transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
        }
            
    }
    // 有标签Player的物体进入触发器
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPressed = true;
            Debug.Log("压力触发！");
            Time.timeScale = 0;
        }
        if (other.CompareTag("Light"))
        {
            isstoped = true;
            Debug.Log("停止");
        }
    }

    // Player离开触发器
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPressed = false;
            Debug.Log("压力解除！");
        }
        if (other.CompareTag("Light"))
        {
            isstoped = false;
            Debug.Log("继续移动");
        }
    }
}
