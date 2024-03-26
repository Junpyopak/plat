using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] GameObject objInventory;
    //[SerializeField] KeyCode keyInventory;
    List<Transform>listInventory = new List<Transform>();
    [SerializeField] GameObject objItem;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        initInventory();
    }

    private void initInventory()
    {
        Transform[]rangeData = objInventory.transform.GetComponentsInChildren<Transform>();
        listInventory.AddRange(rangeData);
        listInventory.RemoveAt(0);
    }

    void Update()
    {
        ActiveInventory();
    }

    //private void OnGUI()//Ű ���ε� �ý���
    //{
    //    Event e = Event.current;
    //    if(e.isKey==true)
    //    {
    //        Debug.Log($"key ={e.keyCode}");
    //    }
    //}

    private void ActiveInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            objInventory.SetActive(!objInventory.activeSelf);//�ؿ� �ڵ� ������
            //if(objInventory.activeSelf==true)
            //{
            //    objInventory.SetActive(false);
            //}
            //else
            //{
            //    objInventory.SetActive(true);
            //}
        }
    }

    /// <summary>
    /// ����ִ� ������ ���Թ�ȣ�� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    private int getEmptyItemSlot()
    {
        int count = listInventory.Count;
        for(int iNum = 0; iNum < count; ++iNum) 
        {
            Transform trsSlot = listInventory[iNum];
            if(trsSlot.childCount==0)
            {
                return iNum;
            }
        }
        return -1;//�������� ������ ����
    }

    public bool GetItem(Sprite _spr)
    {
        int slotNum = getEmptyItemSlot();
        if(slotNum==-1)
        {
           return false;//������ ���� ����
        }

        Instantiate(objItem);


        return true;//�����ۻ�������

    }
}
