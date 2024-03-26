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

    //private void OnGUI()//키 바인딩 시스템
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
            objInventory.SetActive(!objInventory.activeSelf);//밑에 코드 간략히
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
    /// 비어있는 아이템 슬롯번호를 리턴합니다.
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
        return -1;//아이템이 없음을 리턴
    }

    public bool GetItem(Sprite _spr)
    {
        int slotNum = getEmptyItemSlot();
        if(slotNum==-1)
        {
           return false;//아이템 생성 실패
        }

        Instantiate(objItem);


        return true;//아이템생성성공

    }
}
