using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    
    Rigidbody2D rigid;
    Vector2 force;
    bool isRight;
    [SerializeField] float isTriggerTime = 1.0f;
    float timer;
    bool doTrigger = false;
    [SerializeField] Collider2D col;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(doTrigger==false) 
        {
            doTrigger = true;
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rigid.AddForce(force,ForceMode2D.Impulse);//오브젝트를 미는 기능을 넣을때 add.force기능 사용함
    }

    
    void Update()
    {
        // Quaternion 은 곱하기(*=)를 하면 뒤에있는값을 앞에 더한다.
        transform.Rotate(new Vector3(0f, 0f, isRight == true ? -360f : 360f)*Time.deltaTime);

        if(doTrigger==true)
        {
            timer += Time.deltaTime;
            if(timer>=isTriggerTime)
            {
                col.isTrigger = true;
                if (timer >= 3)//타이머가 3초지나면 땅으로 떨어지던 칼들 삭제
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void SetForce(Vector2 _force,bool _isRight)
    {
        force= _force;
        isRight= _isRight;
    }
}
