
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class ChestBox : UdonSharpBehaviour
{
    public Chest chest;
    public GameManager manager;
    private VRCPlayerApi _player;
    private VRCPickup _pickup;
    private Badge _badge;
    private void Update() {
        //Vector3 pos = transform.localPosition;
        //pos.x += 0.01f;
        //
        //transform.localPosition = pos;
    }
    public void OnTriggerEnter(Collider other) {
        if (Networking.GetOwner(other.gameObject) != _player) {
            Debug.Log($"Headbox, You are not Owner");
            return;
        }
        if (manager.player.candidate != (int)CANDIDATE.NOTHING) {
            Debug.Log("your candidate not nothing");
            return;
        }
        
        if (other.name.Contains("President")) {
            Debug.Log("president");
            _badge = other.GetComponent<Badge>();
            if (_badge == null) {
                Debug.Log("_badge is null");   
                return;
            }
            chest.EquipTag((int)CANDIDATE.PRESIDENT);
        } else if (other.name.Contains("Chancellor")) {
            _badge = other.GetComponent<Badge>();
            if (_badge == null) return;
            chest.EquipTag((int)CANDIDATE.CHANCELLOR);
        }
    }
    public void OnTriggerExit(Collider other) {
        if (Networking.GetOwner(other.gameObject) != _player) {
            Debug.Log($"Headbox, You are not Owner");
            return;
        }
        if (manager.player.candidate != (int)CANDIDATE.NOTHING) {
            Debug.Log("your candidate not nothing");
            return;
        }
        chest.ResetIcon();
    }
    public void Attacth() {
        if (!DropBadge(_badge)) {
            Debug.Log($"DropBadge False");
            return;
        }
    }
    public void Detach(int type) {

    }
    private bool DropBadge(Badge badge) {
        _pickup = badge.GetComponent<VRCPickup>();
        if (_pickup == null) return false;
        _pickup.Drop();
        badge.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OnDisableBadge");
        return true;
    }
    private void Start()
    {
        _player = Networking.LocalPlayer;
    }
}
