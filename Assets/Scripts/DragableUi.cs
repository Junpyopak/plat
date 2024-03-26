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
        //순서를 바꾸고싶을땐
        //transform.SetSibling 기능사용

        CanvasGroup.alpha = 0.6f;
        CanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(transform.parent ==canvas)//잘못놓았을때
        {
            transform.SetParent(beforeParent);
            rect.position = beforeParent.GetComponent<RectTransform>().position;//자신이 어디있는지 부모가 알려줌
        }

        CanvasGroup.alpha = 1f;
        CanvasGroup.blocksRaycasts = true;
    }


    private void Awake()//내 기능을 정의할때
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()//타 스크립트에 있는기능을 가져올떄
    {
        canvas = FindObjectOfType<Canvas>().transform;

    }

    public void SetItem(Sprite _spr)
    {
        img.sprite = _spr;
        //img.SetNativeSize();//안에 들어가있는 물체의 사이즈를 자동으로 사이즈 조정할때
    }
}
