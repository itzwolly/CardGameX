using UnityEngine;
using UnityEngine.Networking;

public class NetworkCameraHandler : NetworkBehaviour {
    private Camera _camera;

    public override void OnStartLocalPlayer() {
        _camera = GetComponent<Camera>();
        _camera.enabled = true;
    }

    public void OnDisable() {
        Debug.Log("Camera equals null: " + _camera == null);
    }
}
