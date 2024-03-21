using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    [Header("�÷��̾� �̵��� ����")]
    Rigidbody2D rigid;
    Animator anim;
    BoxCollider2D boxColl;
    [SerializeField] float moveSpeed = 5f;//�÷��̾� �̵��ӵ�
    [SerializeField] float jumpForce = 5f;//����
    [SerializeField] bool isGround;

    bool isJump;
    float verticalValocity = 0f;

    [SerializeField] float rayDistance = 1f;
    [SerializeField] Color rayColor;
    [SerializeField] bool showRay = false;
    Vector3 moveDir;

    [Header("���������")]
    [SerializeField] bool wallStep = false;//�������� �Ҽ��ִ�����
    bool isWallStep;//�߷����ǿ��� �������� �ϰ� ����
    [SerializeField] float wallStepTime = 0.3f;//���ʵ��� ������ �Է��Ҽ� ������ �Ұ�����
    float wallStepTimer = 0.0f;

    [Header("�뽬")]
    [SerializeField] float dashTime = 0.3f;
    float dashTimer = 0.0f;//Ÿ�̸�
    [SerializeField] float dashCoolTime = 2.0f;
    float dashCoolTimer = 0.0f;
    TrailRenderer tr;

    [Header("�뽬��ų ȭ�鿬��")]
    [SerializeField] Image effect;
    [SerializeField] TMP_Text textCool;


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
        
        //ui
        checkUiCoolDown();
    }

    private void checkGrounded()
    {
        isGround = false;

        if (verticalValocity > 0f) return;

        //RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector3.down, rayDistance, LayerMask.GetMask(Tool.GetLayer(Layers.Ground)));
        RaycastHit2D ray = Physics2D.BoxCast(boxColl.bounds.center, boxColl.bounds.size, 0f, Vector2.down, rayDistance, LayerMask.GetMask(Tool.GetLayer(Layers.Ground)));
        if (ray)//�׶��忡 ����
        {
            isGround = true;
        }
    }

    private void moving()
    {
        if (wallStepTimer != 0.0f||dashTimer!=0.0f) return;//���� ������ Ÿ�̸Ӱ� �������̸� �̵��� �Է¹����� ����
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
        //0�϶��� false 1�Ǵ� -1Ʈ��
        anim.SetBool("Walk", moveDir.x != 0.0f);

        if (moveDir.x != 0.0f) //���������ΰ��� x���� 1 ������ x��-1,�������ΰ���  1
        {
            Vector3 locScale = transform.localScale;
            locScale.x = Input.GetAxisRaw("Horizontal") * -1;
            transform.localScale = locScale;

        }

    }

    private void doJump()//������ �����̽��� �������� ����
    {
        if (isGround == false)//���߿� ��������
        {
            if (Input.GetKeyDown(KeyCode.Space) && wallStep == true && moveDir.x != 0)
            {
                isWallStep = true;
            }
        }
        else//�ٴڿ�������
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

            bool dirRight = transform.localScale.x == -1;//�������� �����ִ���
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

        float angle = Quaternion.FromToRotation(Vector3.up, mousPos - transform.position).eulerAngles.z;
        Debug.Log(360-angle);
    }
    private void checkGravity()
    {
        if (dashTimer != 0.0f) return;

        if (isWallStep == true)
        {

            isWallStep = false; 

            Vector2 dir = rigid.velocity;
            dir.x *= -1;
            rigid.velocity = dir;//���� �����ִ¹����� �ݴ�
            verticalValocity = jumpForce;//������

            wallStepTimer = wallStepTime;//������ �ԷºҰ� ���ð��� Ÿ�̸ӿ� �Է�
        }
        else if (isGround == false)//ĳ���Ͱ� ���߿� ������
        {
            verticalValocity -= 9.81f * Time.deltaTime;
            if (verticalValocity < -10.0f)
            {
                verticalValocity = -10.0f;
            }
        }
        else//���� �پ�������
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
