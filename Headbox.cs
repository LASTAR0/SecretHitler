
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class Headbox : UdonSharpBehaviour
{
    public GameManager manager;
    public Chest chest;
    private VRCPlayerApi _player;

    private void Update() {
        Vector3 pos = _player.GetBonePosition(HumanBodyBones.Head);
        pos.y += 0.3f;
        transform.position = pos;
    }
    private void OnTriggerEnter(Collider other) {
        if (manager.player.candidate == (int)CANDIDATE.NOTHING) 
            return;
        if (other.name.Contains("HandBox")) {
            HandBox box = other.GetComponent<HandBox>();
            if (box == null) return;

            chest.UnEquipTag();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (manager.player.candidate == (int)CANDIDATE.NOTHING) 
            return;
        if (other.name.Contains("HandBox")) {
            HandBox box = other.GetComponent<HandBox>();
            if (box == null) return;

            chest.ResetIcon();
        }
    }
    private void Start()
    {
        _player = Networking.LocalPlayer;
    }
}
