using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleOnMouseEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool openUIInfo;
    public void OnPointerEnter(PointerEventData eventData)
    {
        openUIInfo = true;

        UIManager.current.ShowTrackInfo();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        openUIInfo = false;
    }


}
