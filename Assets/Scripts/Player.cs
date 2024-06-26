using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    [Header("플레이어 이동과 점프")]
    Rigidbody2D rigid;
    Animator anim;
    BoxCollider2D boxColl;
    [SerializeField] float moveSpeed = 5f;//플레이어 이동속도
    [SerializeField] float jumpForce = 5f;//점프
    [SerializeField] bool isGround;

    bool isJump;
    float verticalValocity = 0f;

    [SerializeField] float rayDistance = 1f;
    [SerializeField] Color rayColor;
    [SerializeField] bool showRay = false;
    Vector3 moveDir;

    [Header("벽점프기능")]
    [SerializeField] bool wallStep = false;//벽점프를 할수있는조건
    bool isWallStep;//중력조건에서 벽점프를 하게 할지
    [SerializeField] float wallStepTime = 0.3f;//몇초동안 유저가 입력할수 없도록 할것인지
    float wallStepTimer = 0.0f;

    [Header("대쉬")]
    [SerializeField] float dashTime = 0.3f;
    float dashTimer = 0.0f;//타이머
    [SerializeField] float dashCoolTime = 2.0f;
    float dashCoolTimer = 0.0f;
    TrailRenderer tr;

    [Header("대쉬스킬 화면연출")]
    [SerializeField] Image effect;
    [SerializeField] TMP_Text textCool;

    [Header("무기투척")]
    [SerializeField] Transform trsHand;
    [SerializeField] GameObject objSword;
    [SerializeField] Transform trsSword;//칼의 위치와 각도를 가져오는곳
    [SerializeField] float throwForce;
    bool isRight;
    private void OnDrawGizmos()
    {
        if (showRay == true)
        {
            Gizmos.color = rayColor;
            Gizmos.DrawLine(transform.position, transform.position - new Vector3(0, rayDistance));
        }
    }
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
        tr = GetComponent<TrailRenderer>();
        tr.enabled = false;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkGrounded();
        moving();
        doJump();
        checkAim();
        checkGravity();
        doDash();
        checkTimers();
        shootWeapon();
        //ui
        checkUiCoolDown();
    }

    private void checkGrounded()
    {
        isGround = false;

        if (verticalValocity > 0f) return;

        //RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector3.down, rayDistance, LayerMask.GetMask(Tool.GetLayer(Layers.Ground)));
        RaycastHit2D ray = Physics2D.BoxCast(boxColl.bounds.center, boxColl.bounds.size, 0f, Vector2.down, rayDistance, LayerMask.GetMask(Tool.GetLayer(Layers.Ground)));
        if (ray)//그라운드에 닿음
        {
            isGround = true;
        }
    }

    private void moving()
    {
        if (wallStepTimer != 0.0f||dashTimer!=0.0f) return;//만약 벽점프 타이머가 구동중이면 이동을 입력받을수 없음
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
        //0일때는 false 1또는 -1트루
        anim.SetBool("Walk", moveDir.x != 0.0f);
        anim.SetBool("Jump", moveDir.y !=0f);

        //if (moveDir.x != 0.0f) //오른쪽으로갈땐 x값은 1 스케일 x는-1,왼쪽으로갈때  1
        //{
        //    Vector3 locScale = transform.localScale;
        //    locScale.x = Input.GetAxisRaw("Horizontal") * -1;
        //    transform.localScale = locScale;

        //}

    }

    private void doJump()//유저가 스페이스를 눌렀을때 점프
    {
        if (isGround == false)//공중에 떠있을때
        {
            if (Input.GetKeyDown(KeyCode.Space) && wallStep == true && moveDir.x != 0)
            {
                isWallStep = true;
            }
        }
        else//바닥에있을때
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJump = true;
            }
        }
    }

    private void doDash()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)&&dashTimer==0.0f&&dashCoolTimer==0.0f)
        {
            
            verticalValocity = 0.0f;

            bool dirRight = transform.localScale.x == -1;//오른쪽을 보고있는지
            rigid.velocity = new Vector2(dirRight == true ? 20.0f : -20.0f, verticalValocity);

            dashTimer = dashTime;
            dashCoolTimer = dashCoolTime;
            
            tr.enabled = true;
        }
    }

    private void checkAim()
    {
        Vector3 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousPos.z = transform.position.z;
        Vector3 newPos = mousPos - transform.position;
        isRight = newPos.x >0 ? true : false;

        if(newPos.x>0&&transform.localScale.x!=-1.0f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);          
        }
        else if (newPos.x<0&&transform.localScale.x!=1.0f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);           
        }

        Vector3 direction = isRight == true ? Vector3.right : Vector3.left;
        float angle = Quaternion.FromToRotation(direction, newPos).eulerAngles.z;
        angle = isRight == true ? -angle : angle;

        //if(isRight ==true)//if문으로도 가능
        //{
        //    angle = -angle;
        //}

        trsHand.localRotation = Quaternion.Euler(0, 0, angle);//rotation은 월드 local은 부모를 기준으로 내 위치를 정할떄
        //trsHand.localEulerAngles = new Vector3(0, 0, angle);
        //trsHand.eulerAngles = new Vector3(0, 0, angle);
    }

    private void shootWeapon()
    {

        if(Input.GetKeyDown(KeyCode.Mouse0)&&InventoryManager.Instance.isActiveInventory()==false)
        {
            GameObject go = Instantiate(objSword, trsSword.position, trsSword.rotation);
            ThrowWeapon gosc = go.GetComponent<ThrowWeapon>();
            //addForce에서는 힘을전달해줄때 월드포스값으로 힘을줘야 움직임.
            Vector2 throwForce = isRight == true ? new Vector2(10f, 0f) : new Vector2(-10f, 0.0f);
            gosc.SetForce(trsSword.rotation * throwForce, isRight);
        }
    }

    private void checkGravity()
    {
        if (dashTimer != 0.0f) return;

        if (isWallStep == true)
        {

            isWallStep = false; 

            Vector2 dir = rigid.velocity;
            dir.x *= -1;
            rigid.velocity = dir;//현재 보고있는방향의 반대
            verticalValocity = jumpForce;//점프력

            wallStepTimer = wallStepTime;//벽점프 입력불가 대기시간을 타이머에 입력
        }
        else if (isGround == false)//캐릭터가 공중에 있을떄
        {
            verticalValocity -= 9.81f * Time.deltaTime;
            if (verticalValocity < -10.0f)
            {
                verticalValocity = -10.0f;
            }
        }
        else//땅에 붙어있을떄
        {
            if (isJump == true)
            {
                isJump = false;
                verticalValocity = jumpForce;
            }

            else if (verticalValocity < 0)
            {
                verticalValocity = 0f;
            }
        }

        rigid.velocity = new Vector2(rigid.velocity.x, verticalValocity);
    }

    public void TriggerEnter(HitBox.enumHitType _hitType, Collider2D _coll)
    {
        switch (_hitType)
        {
            case HitBox.enumHitType.WallCheck:
                wallStep = true;
                break;
            case HitBox.enumHitType.ItemCheck:
                //지금 닿은 대상이 item이 맞는지
                ItemSetting item = _coll.GetComponent<ItemSetting>();

                //if(_coll.gameObject.tag =="Item")//테그를 체크하는방법
                //if(_coll.gameObject.layer==LayerMask.NameToLayer(""))
                if(item != null)
                {
                    item.GetItem();
                }
                break;
            case HitBox.enumHitType.JumpBoardCheck://점프보드 닿았을때
                break;
        }
    }

    public void TriggerExit(HitBox.enumHitType _hitType, Collider2D _coll)
    {
        switch (_hitType)
        {
            case HitBox.enumHitType.WallCheck:
                wallStep = false;
                break;
        }
    }

    private void checkTimers()
    {
        if(wallStepTimer>0.0f)
        {
            wallStepTimer -= Time.deltaTime;
            if(wallStepTimer < 0.0f)
            {
                wallStepTimer = 0.0f;
            }
        }

        if(dashTimer>0.0f)
        {
           dashTimer-= Time.deltaTime;
            if(dashTimer < 0.0f)
            {
                dashTimer = 0.0f;
                tr.enabled = false;
                tr.Clear();
            }
        }
        if(dashCoolTimer>0.0f)
        {
            dashCoolTimer -= Time.deltaTime;
            if(dashCoolTimer < 0.0f)
            {
                dashCoolTimer = 0.0f;
            }
        }
    }
    private void checkUiCoolDown()
    {
        textCool.gameObject.SetActive(dashCoolTimer!=0.0f);
        textCool.text =(Mathf.CeilToInt(dashCoolTimer)).ToString();

        float amount = 1- dashCoolTimer / dashCoolTime;
        effect.fillAmount = amount;
    }
}
