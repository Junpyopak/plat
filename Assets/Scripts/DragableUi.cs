using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragableUi : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform canvas;
    Transform beforeParent;
    RectTransform rect;
    CanvasGroup CanvasGroup;
    Image img;

    public void OnBeginDrag(PointerEventData eventData)
    {
        beforeParent = transform.parent;

        transform.SetParent(canvas);
        transform.SetAsLastSibling();
        //������ �ٲٰ������
        //transform.SetSibling ��ɻ��

        CanvasGroup.alpha = 0.6f;
        CanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(transform.parent ==canvas)//�߸���������
        {
            transform.SetParent(beforeParent);
            rect.position = beforeParent.GetComponent<RectTransform>().position;//�ڽ��� ����ִ��� �θ� �˷���
        }

        CanvasGroup.alpha = 1f;
        CanvasGroup.blocksRaycasts = true;
    }


    private void Awake()//�� ����� �����Ҷ�
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()//Ÿ ��ũ��Ʈ�� �ִ±���� �����Ë�
    {
        canvas = FindObjectOfType<Canvas>().transform;

    }

    public void SetItem(Sprite _spr)
    {
        img.sprite = _spr;
        //img.SetNativeSize();//�ȿ� ���ִ� ��ü�� ����� �ڵ����� ������ �����Ҷ�
    }
}
