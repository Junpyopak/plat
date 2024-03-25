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
        rigid.AddForce(force,ForceMode2D.Impulse);//������Ʈ�� �̴� ����� ������ add.force��� �����
    }

    
    void Update()
    {
        // Quaternion �� ���ϱ�(*=)�� �ϸ� �ڿ��ִ°��� �տ� ���Ѵ�.
        transform.Rotate(new Vector3(0f, 0f, isRight == true ? -360f : 360f)*Time.deltaTime);

        if(doTrigger==true)
        {
            timer += Time.deltaTime;
            if(timer>=isTriggerTime)
            {
                col.isTrigger = true;
                if (timer >= 3)//Ÿ�̸Ӱ� 3�������� ������ �������� Į�� ����
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
