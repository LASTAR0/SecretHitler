
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class JoinTrigBox : UdonSharpBehaviour
{
    public PlayerManager playerManager;

    // [Master]
    public override void OnPlayerTriggerEnter(VRCPlayerApi player) {
        if (Networking.LocalPlayer.isMaster && !playerManager.manager.isPlaying) {
            playerManager.addPlayer(player);
        }
    }
    public override void OnPlayerTriggerExit(VRCPlayerApi player) {
        if (Networking.LocalPlayer.isMaster && !playerManager.manager.isPlaying) {
            playerManager.subPlayer(player);
        }
    }
}
