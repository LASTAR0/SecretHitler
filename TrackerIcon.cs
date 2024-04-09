
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TrackerIcon : UdonSharpBehaviour
{
    public UnityEngine.UI.Image image;
    private VRCPlayerApi _player;
    private void Update() {
        Vector3 newPos = _player.GetBonePosition(HumanBodyBones.Head) - transform.position;
        Quaternion look = Quaternion.LookRotation(newPos, Vector3.up);
        transform.rotation = look;
    }
    private void Start()
    {
        _player = Networking.LocalPlayer;
    }
}
