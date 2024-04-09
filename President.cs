
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class President : UdonSharpBehaviour
{
    private VRCPlayerApi _target;
    private VRCPlayerApi _localPlayer;
    private bool _isEnabled = false;
    private void Update() {
        Vector3 pos = _target.GetBonePosition(HumanBodyBones.Head);
        pos.y += 0.6f;
        transform.position = pos;

        Vector3 newPos = _localPlayer.GetBonePosition(HumanBodyBones.Head) - transform.position;
        Quaternion look = Quaternion.LookRotation(newPos, Vector3.up);
        transform.rotation = look;
    }

    public void EnablePresident(int id) {
        gameObject.SetActive(true);
        _isEnabled = true;
        _target = VRCPlayerApi.GetPlayerById(id);
        _localPlayer = Networking.LocalPlayer;
    }
    public void Disable() {
        gameObject.SetActive(false);
        _isEnabled = false;
    }
}
