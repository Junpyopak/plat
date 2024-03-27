using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum enumHitType
    {
        WallCheck,
        ItemCheck,
        JumpBoardCheck,
    }
    [SerializeField] private enumHitType hitType;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        //player = transform.parent.GetComponent<Player>();
        player = GetComponentInParent<Player>();//내 부모한테 있는게 확실하다묜
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.TriggerEnter(hitType, collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        player.TriggerExit(hitType, collision);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
