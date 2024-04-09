
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Descriptor : UdonSharpBehaviour
{
    public float walkSpeed = 2.0f;
    public float runSpeed = 3.0f;
    public float jump = 3.0f;

    
    void Start()
    {
        VRCPlayerApi player = Networking.LocalPlayer;
        if (player != null) {
            player.SetWalkSpeed(walkSpeed);
            player.SetRunSpeed(runSpeed);
            player.SetJumpImpulse(jump);
        }
    }
}
