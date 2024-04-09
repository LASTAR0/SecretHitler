
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PlayTrigBox : UdonSharpBehaviour
{
    public LobbyUI lobbyUI;
    public Image imgIcon;
    private const float FadeDuration = 3.0f;
    private bool _isEntering = false;
    private float _fill = 0.0f;
    private void Update() {
        if (!_isEntering) return;

        _fill += Time.deltaTime / FadeDuration;
        _fill = Mathf.Clamp01(_fill);
        imgIcon.fillAmount = _fill;
        if (imgIcon.fillAmount >= 1.0f) {
            ExitTrigBox();
            lobbyUI.manager.OnPlay();
            lobbyUI.RequestSerialization();
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (!Networking.LocalPlayer.isMaster) return;

        if (other.name.Contains("HandBox")) {
            HandBox box = other.GetComponent<HandBox>();
            if (box == null) return;
            EnterTrigBox();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!Networking.LocalPlayer.isMaster) return;

        if (other.name.Contains("HandBox")) {
            HandBox box = other.GetComponent<HandBox>();
            if (box == null) return;
            ExitTrigBox();
        }
    }
    private void EnterTrigBox() {
        _isEntering = true;
    }
    private void ExitTrigBox() {
        _isEntering = false;
        _fill = 0.0f;
        imgIcon.fillAmount = _fill;
    }
}
