using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    //������Ʈ�� �̴� ����� ������ add.force��� �����
    Rigidbody2D rigid;
    Vector2 force;
    bool isRight;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rigid.AddForce(force,ForceMode2D.Impulse);
    }

    
    void Update()
    {
        // Quaternion �� ���ϱ�(*=)�� �ϸ� �ڿ��ִ°��� �տ� ���Ѵ�.
        transform.Rotate(new Vector3(0f, 0f, isRight == true ? -360f : 360f));
    }

    public void SetForce(Vector2 _force,bool _isRight)
    {
        force= _force;
        isRight= _isRight;
    }
}
