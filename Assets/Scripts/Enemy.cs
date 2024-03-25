using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed;

    Rigidbody2D rigid;
    Collider2D groundCheckCol;//땅을 체크하기위한

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        groundCheckCol = transform.GetChild(0).GetComponent<Collider2D>();
    }

    private void FixedUpdate()//defult,0.02초에 한번씩
    {
        if (groundCheckCol.IsTouchingLayers(LayerMask.GetMask("Ground")) == false)
        {
            flip();
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
