using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D rigid;
    Collider2D groundCheckCol;//땅을 체크하기위한
    Collider2D groundCheck; //enemy에 설정된 콜라이더로 땅을 밟고있는지 확인을 위함

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        groundCheckCol = transform.GetChild(0).GetComponent<Collider2D>();
        groundCheck = GetComponent<Collider2D>();
        
    }

    private void FixedUpdate()//defult,0.02초에 한번씩
    {
        if(groundCheck.IsTouchingLayers(LayerMask.GetMask("Ground"))==true)//enemy에 설정된 콜라이더로 땅을 밟고 있으면서
                                                                           //땅체크하는 체크박스가 ground에서 떨어지면flip함수 실행(공중에 있을때 부들거리며 떨어지는걸 방지)
        {
            if (groundCheckCol.IsTouchingLayers(LayerMask.GetMask("Ground")) == false)//땅 체크하는 체크박스가 ground에서 떨어지면flip함수 실행
            {
                flip();
            }
        }
    }

    private void flip()//땅이 없으면 뒤로 돌기
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
