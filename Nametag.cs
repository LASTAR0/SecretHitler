
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Nametag : UdonSharpBehaviour
{
    private bool _isEnabled = false;
    private VRCPlayerApi _hitlerPlayer;
    private VRCPlayerApi _localPlayer;
    
    public void Update() {
        if (_hitlerPlayer == null) return;
        if (_isEnabled) {
            Vector3 pos = _hitlerPlayer.GetBonePosition(HumanBodyBones.Head);
            pos.y += 0.4f;
            transform.position = pos;

            Vector3 newPos = _localPlayer.GetBonePosition(HumanBodyBones.Head) - transform.position;
            Quaternion look = Quaternion.LookRotation(newPos, Vector3.up);
            transform.rotation = look;
        }

    }
    public void OnEnableMark(int id) {
        gameObject.SetActive(true);
        _isEnabled = true;
        _hitlerPlayer = VRCPlayerApi.GetPlayerById(id);
        _localPlayer = Networking.LocalPlayer;
    }
    public void OnDisableMark() {
        gameObject.SetActive(false);
        _isEnabled = false;
    }
}
