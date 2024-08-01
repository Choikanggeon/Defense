using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BTNType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ButtonType currentType;
    public Transform buttonScale;
    Vector3 defaultScale;

    private void Start()
    {
        defaultScale = buttonScale.localScale;
    }
    public void OnBttonClick()
    {
        switch (currentType)
        {
            case ButtonType.New:
                Debug.Log("������");
                break;
            case ButtonType.Continue:
                Debug.Log("�̾��ϱ�");
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale;
    }
}
