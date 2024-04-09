
using librsync.net;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BoardTrigBox : UdonSharpBehaviour
{
    public Board board;
    // [Bullet Owner]
    public void OnTriggerEnter(Collider other) {
        if (!except(other)) return;

        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        VRC_Pickup pickup = other.GetComponent<VRC_Pickup>();
        if (!rigidbody || !pickup) return;

        Debug.Log(rigidbody.velocity.magnitude);
        if (rigidbody.velocity.magnitude >= 0.7f) {
            pickup.Drop();
            Bullet bullet = other.GetComponent<Bullet>();
            Magnifying magnifying = other.GetComponent<Magnifying>();
            if (bullet) {
                Debug.Log("Bullet");
                bullet.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Disable");
                board.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "KillPlayer");
            } else if (magnifying) {
                magnifying.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Disable");
                board.EnableMemberText();
            }
        }
    }

    // [Except]
    private bool except(Collider other) {
        if (!other.gameObject.name.Contains("Bullet") && !other.gameObject.name.Contains("Magnifying")) return false;
        if (Networking.LocalPlayer != Networking.GetOwner(other.gameObject)) return false;
        if (board.id == -1) return false;
        
        //if (board.boardManager.manager.player.candidate != (int)CANDIDATE.PRESIDENT) return false;
        return true;
    }
}
