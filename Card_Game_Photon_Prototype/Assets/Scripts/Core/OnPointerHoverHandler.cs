using UnityEngine;
using UnityEngine.EventSystems;

public class OnPointerHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [HideInInspector] public bool Hover;

    public void OnPointerEnter(PointerEventData eventData) {
        Hover = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        Hover = false;
    }
}
