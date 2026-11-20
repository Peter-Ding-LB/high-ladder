using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{

    // Start is called before the first frame update
    public float speed;//角色速度设置

    public Vector2 collidersize;
    public GameObject flashlight;//手电筒组件

    public Light2D selflight;

    private Rigidbody2D rg;
    
    private SpriteRenderer sprite;
    
    private CapsuleCollider2D capsuleCollider;
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        collidersize = capsuleCollider.size;
    }

    // Update is called once per frame
    void Update()
    {
        float a = Input.GetAxis("Horizontal")*speed;
        rg.velocity = new Vector2 (a, 0);//a和d键控制角色
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//鼠标坐标
        Vector2 flashpos = new Vector2(flashlight.transform.position.x, flashlight.transform.position.y);//手电筒坐标
        float flashangle = Angle(mousepos, flashpos);//手电筒和鼠标形成的夹角
        float mouseposx = mousepos.x - flashpos.x;
        float mouseposy = mousepos.y - flashpos.y;
        float flashditance = Mathf.Sqrt(mouseposx*mouseposx+mouseposy*mouseposy);
        selflight.pointLightInnerRadius = flashditance;
       flashlight.transform.rotation = Quaternion.Euler(0,0,flashangle);//手电筒跟随鼠标
        if (a > 0 || (flashangle < 90 && flashangle > -90))
        {
            sprite.flipX = true;
         }
        else
        {
            sprite.flipX=false;
        }//角色翻转
    }
    public float Angle(Vector2 a,Vector2 b)
    {

        return Mathf.Atan2(a.y-b.y,a.x-b.x)*Mathf.Rad2Deg;//计算夹角的方法
    }
   
    private void SlopCheck()
    {
        
    }
}
