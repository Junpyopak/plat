using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D rigid;
    Collider2D groundCheckCol;//���� üũ�ϱ�����
    CapsuleCollider2D groundCheck; //enemy�� ������ ĸ�� �ݶ��̴��� ���� ����ִ��� Ȯ���� ����

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        groundCheckCol = transform.GetChild(0).GetComponent<Collider2D>();
        groundCheck = GetComponent<CapsuleCollider2D>();
        
    }

    private void FixedUpdate()//defult,0.02�ʿ� �ѹ���
    {
        if(groundCheck.IsTouchingLayers(LayerMask.GetMask("Ground"))==true)//enemy�� ������ ĸ�� �ݶ��̴��� ���� ��� �����鼭
                                                                           //��üũ�ϴ� üũ�ڽ��� ground���� ��������flip�Լ� ����(���߿� ������ �ε�Ÿ��� �������°� ����)
        {
            if (groundCheckCol.IsTouchingLayers(LayerMask.GetMask("Ground")) == false)//�� üũ�ϴ� üũ�ڽ��� ground���� ��������flip�Լ� ����
            {
                flip();
            }
        }
            //if (groundCheckCol.IsTouchingLayers(LayerMask.GetMask("Ground")) == false)//�� üũ�ϴ� üũ�ڽ��� ground���� ��������flip�Լ� ����
            //{
            //    flip();
            //}

    }

    private void flip()//���� ������ �ڷ� ����
    {
        speed *= -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }


    private void Update()
    {
        rigid.velocity = new Vector2(speed,rigid.velocity.y);

    }
}
