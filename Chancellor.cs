
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Chancellor : UdonSharpBehaviour
{
    private VRCPlayerApi _target;
    private VRCPlayerApi _localPlayer;
    private void Update() {
        Vector3 pos = _target.GetBonePosition(HumanBodyBones.Head);
        pos.y += 0.6f;
        transform.position = pos;

        Vector3 newPos = _localPlayer.GetBonePosition(HumanBodyBones.Head) - transform.position;
        Quaternion look = Quaternion.LookRotation(newPos, Vector3.up);
        transform.rotation = look;
    }

    public void EnableChancellor(int id) {
        gameObject.SetActive(true);
        _target = VRCPlayerApi.GetPlayerById(id);
        _localPlayer = Networking.LocalPlayer;
    }

    public void Enable() {
        gameObject.SetActive(true);
    }
    public void Disable() {
        gameObject.SetActive(false);
    }
}
