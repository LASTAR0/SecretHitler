
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public enum MODE {
    MIN = 0,
    AVERAGE = 1,
    MAX = 2
}
public class BriefingManager : UdonSharpBehaviour
{
    public GameManager manager;
    public Briefing fascist;
    public BriefingCard[] fascistCards, liberalCards;

    // Not sync
    [HideInInspector] public int numFascist, numLiberal;

    // 0 = 5~6 // 1 = 7~8 // 2 = 9~10
    [UdonSynced, HideInInspector] public int briefingType = 0;

    public void ChangeBriefingMaterial() {
        int num = manager.playerManager.playerPool.Length;
        if (num > 4 && num < 7) { briefingType = (int)MODE.MIN; }
        else if (num > 6 && num < 9) { briefingType = (int)MODE.AVERAGE; }
        else { briefingType = (int)MODE.MAX; }

        //briefingType = (int)MODE.AVERAGE;       // FOR TEST

        RequestSerialization();
    }
    // [Master]Assets/Resources/Script/Manager/BriefingManager.cs
    public void AddPolicyFascist() {
        if (numFascist < fascistCards.Length) {
            fascistCards[numFascist++].SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Enable");
            
            if (briefingType == (int)MODE.AVERAGE && numFascist == 2) {
                // Mag
                manager.propManager.EnableMagnifying(0, true);
            } else if (briefingType == (int)MODE.MAX) {
                if (numFascist == 1 || numFascist == 2) {
                    manager.propManager.EnableMagnifying(numFascist - 1 , true);
                }
            }
            if (numFascist == 4 || numFascist == 5) {
                // Bullet Enable
                manager.propManager.EnableBullet(numFascist - 4, true);
            }
            // Vicotry Condition
            if (numFascist >= fascistCards.Length) {
                manager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "FascistVictory");
            }
        }
    }
    public void AddPolicyLiberal() {
        if (numLiberal < liberalCards.Length) {
            liberalCards[numLiberal++].SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Enable");
            if (numLiberal >= liberalCards.Length) {
                manager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "LiberalVictory");
            }
        }
    }

    public override void OnPreSerialization() { Sync(); }
    public override void OnDeserialization() { Sync(); }
    private void Sync() {
        Debug.Log($"Change Material {briefingType}");
        fascist.ChangeMaterial(briefingType);
    }
}
