using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{

    // Start is called before the first frame update
    public float speed;//角色速度设置
    public float repairspeed;
    public float Jumppower;
    public float offset;

    public GameObject flashlight;//手电筒组件
    public GameObject Door;
    public GameObject Exit;
    public GameObject props;

    public Animator playeranimator;

    public Sprite repairsprite;
    

    public GameTest gameui;
    public Light2D selflight;
    public CameraFollow cameras;
    public PressureZone pressure;

    private Rigidbody2D rg;
    
    private SpriteRenderer sprite;
    
    private CapsuleCollider2D capsuleCollider;

    public LayerMask whatIsGround;

    public List<RepairTest> repairs;
    private float slopecheckDistance = 0.5f;

    private bool isground = false;
    private Vector2 hitnormal;
    private Vector2 collidersize;
    private Vector2 dir;
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
        dir = new Vector2(a, 0);
        transform.Translate(dir*Time.deltaTime);//a和d键控制角色
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//鼠标坐标
        Vector2 flashpos = new Vector2(flashlight.transform.position.x, flashlight.transform.position.y);//手电筒坐标
        float flashangle = Angle(flashpos,mousepos);//手电筒和鼠标形成的夹角
        float flashditance = Vector2.Distance(mousepos,flashpos);
        selflight.pointLightInnerRadius = flashditance;
       flashlight.transform.rotation = Quaternion.Euler(0,0,flashangle+90);//手电筒跟随鼠标
        if (a > 0 || (flashangle > 90 ||flashangle < -90))
        {
            sprite.flipX = true;
         }
        else
        {
            sprite.flipX=false;
        }//角色翻转
        SlopCheck();
        if (Input.GetKeyDown(KeyCode.Space) && isground)
        {
            rg.AddForce(Vector2.up*Jumppower);
            
        }
        if (a!=0)
        {
            playeranimator.SetBool("Walk",true);
        }
        if (a==0)
        {
            playeranimator.SetBool("Walk", false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playeranimator.SetBool("Jump", true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            playeranimator.SetBool("Jump", false);
        }
    }

    public float Angle(Vector2 a,Vector2 b)
    {

        return Mathf.Atan2( a.y-b.y, a.x - b.x) *Mathf.Rad2Deg;//计算夹角的方法
    }
   
    private void SlopCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0f,collidersize.y/2);
        if (SlopCheckVertical(checkPos))
        {
            dir = dir -hitnormal*Vector2.Dot(dir,hitnormal);
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "LightProp")
        {
            flashlight.gameObject.SetActive(true);
            pressure.Eable();
            Destroy(props.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Repair")
        {
            gameui.StartRead();
            flashlight.gameObject.SetActive(false);
            if (Input.GetKey(KeyCode.E))
            {
                sprite.sprite = repairsprite;
                RepairTest target = collision.GetComponent<RepairTest>();
                gameui.ReadLine(repairspeed * Time.deltaTime, target);
            }
           
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Repair")
        {
            gameui.FinishRead();
            flashlight.gameObject.SetActive(true);
        }
        if (collision.gameObject.tag == "Exit")
        {
            cameras.IsElevator(true);
            pressure.Stop();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isground = true;
        }
        if (collision.transform.tag == "Door")
        {
            transform.position = Exit.transform.position;
           
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isground = false;
        }
    }
    public void Repair(RepairTest repair)
    {
        repairs.Remove(repair);
        if (repairs.Count == 0)
        {
            Door.gameObject.SetActive(true);
        }
    }
}
