using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    //오브젝트를 미는 기능을 넣을때 add.force기능 사용함
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
        // Quaternion 은 곱하기(*=)를 하면 뒤에있는값을 앞에 더한다.
        transform.Rotate(new Vector3(0f, 0f, isRight == true ? -360f : 360f));
    }

    public void SetForce(Vector2 _force,bool _isRight)
    {
        force= _force;
        isRight= _isRight;
    }
}
