using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetting : MonoBehaviour
{
    [SerializeField] Sprite sprInven;//�κ��丮���� ���� �̹���
    public void GetItem()
    {
        if(InventoryManager.Instance.GetItem(sprInven))//Ʈ���� �������� ����������
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("������â�� ����á��");
        }
    }
}
