using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnPointerClickHandler : MonoBehaviour, IPointerClickHandler {
    public UnityEvent OnLeftClick;
    public UnityEvent OnRightClick;

    public void OnPointerClick(PointerEventData pEventData) {
        if (pEventData.button == PointerEventData.InputButton.Left) {
            OnLeftClick.Invoke();
        } else if (pEventData.button == PointerEventData.InputButton.Right) {
            OnRightClick.Invoke();
        }
    }
}
