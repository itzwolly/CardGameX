using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSlotBehaviour : MonoBehaviour {
    [SerializeField] private Material _occupiedMaterial;

    private MeshRenderer _meshRenderer;

    private void Start() {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonUp(0)) {
            if (transform.tag == "Interactable") {
                Events.RaiseCardPlayedEvent(transform.GetSiblingIndex(), PhotonNetwork.player.ID);
            }
        }
    }

    public void OccupyBoardSlot() {
        _meshRenderer.material = _occupiedMaterial;
    }
}
