using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public bool mouse_over = false;


    public void OnPointerEnter(PointerEventData eventData)//ui에 마우스 커서가 올라왔을때
    {
        mouse_over = true;
        UIManager.main.SetHoveringState(true);
    }

    public void OnPointerExit(PointerEventData eventData)//ui에 마우스 커서가 내려갓을때
    {
        mouse_over = false;
        UIManager.main.SetHoveringState(false);
        gameObject.SetActive(false);

    }
}
