
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Magnifying : UdonSharpBehaviour
{
    public PropManager propManager;
    public int index = 0;
    public void Disable() {
        if (Networking.LocalPlayer.isMaster) {
            propManager.EnableMagnifying(index, false);
        }
    }
}
