
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Door : UdonSharpBehaviour
{
    public BoardManager boardManager;

    // Teleport To Board Owner Position
    public override void Interact() {
        foreach (Board b in boardManager.boards) {
            if (b.id == Networking.LocalPlayer.playerId) {
                Networking.LocalPlayer.TeleportTo(b.trSpawn.position, b.transform.rotation);
            }
        }
    }
}
