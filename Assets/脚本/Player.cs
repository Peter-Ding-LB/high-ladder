using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{

    // Start is called before the first frame update
    public float speed;//角色速度设置
    
    public GameObject flashlight;//手电筒组件

    public Light2D selflight;

    private Rigidbody2D rg;
    
    private SpriteRenderer sprite;
    
    private CapsuleCollider2D capsuleCollider;

    public LayerMask whatIsGround;

    private float slopecheckDistance = 0.5f;
    private float hp = 2;

    private Vector2 hitnormal;
    private Vector2 collidersize;
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        collidersize = capsuleCollider.size;
        Debug.Log(collidersize);
    }

    // Update is called once per frame
    void Update()
    {
        float a = Input.GetAxis("Horizontal")*speed;
        rg.velocity = new Vector2 (a, 0);//a和d键控制角色
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//鼠标坐标
        Vector2 flashpos = new Vector2(flashlight.transform.position.x, flashlight.transform.position.y);//手电筒坐标
        float flashangle = Angle(mousepos, flashpos);//手电筒和鼠标形成的夹角
        float flashditance = Vector2.Distance(mousepos,flashpos);
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
        SlopCheck();
       
       
    }
    
    public float Angle(Vector2 a,Vector2 b)
    {

        return Mathf.Atan2(a.y-b.y,a.x-b.x)*Mathf.Rad2Deg;//计算夹角的方法
    }
   
    private void SlopCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0f,collidersize.y/2);
        if (SlopCheckVertical(checkPos))
        {
            rg.velocity = rg.velocity -hitnormal*Vector2.Dot(rg.velocity,hitnormal);
        }
    }
    private void SlopCheckHorizantal(Vector2 checkPos)
    {
        
    }
    private bool SlopCheckVertical(Vector2 checkPos) 
    {
       RaycastHit2D hit = Physics2D.Raycast(checkPos,Vector2.down,slopecheckDistance,whatIsGround);
        if (hit)
        {
            hitnormal = hit.normal;
           Vector2 slopdown = Vector2.Perpendicular(hit.normal);
            float slopeangle = Vector2.Angle(hit.normal, Vector2.up);
            Debug.DrawRay(hit.point,slopdown,Color.blue);
            Debug.DrawRay(hit.point,hit.normal,Color.red);
        }
        return true;
    }

}
