
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HandBox : UdonSharpBehaviour
{
    private VRCPlayerApi _player;
    private void Update() {
        Vector3 pos = _player.GetBonePosition(HumanBodyBones.RightHand);
        transform.position = pos;
    }
    private void Start()
    {
        _player = Networking.LocalPlayer;
    }
}
