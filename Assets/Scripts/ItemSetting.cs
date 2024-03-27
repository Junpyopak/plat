using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetting : MonoBehaviour
{
    [SerializeField] Sprite sprInven;//인벤토리에서 나올 이미지
    public void GetItem()
    {
        if(InventoryManager.Instance.GetItem(sprInven))//트루라면 아이템을 넣을수있음
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("아이템창이 가득찼음");
        }
    }
}
