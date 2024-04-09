
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PrivateRoom : UdonSharpBehaviour
{
    public PrivateManager privateManager;
    [HideInInspector] public bool isJoined = false;
    [UdonSynced, HideInInspector] public int currentPlayer = 0;

    public Transform trSpawn;
    public Clipboard clipboardRole;
    public Clipboard clipboardMember;

    public string[] funcChangeFascist;
    public string[] funcChangeLiberal;
    
    public void JoinPlayer(int id) {
        isJoined = true;
        currentPlayer = id;
        //Debug.Log($"PrivateRoom, JoinPlayer, id : {id}");
        RequestSerialization();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Teleport");
        ChangeClipboardMaterials(id);
    }
    public void Teleport() {
        if (Networking.LocalPlayer.playerId == currentPlayer) {
            //Debug.Log($"PrivateRoom, Teleport, currentPlayer : {currentPlayer} , name : {Networking.LocalPlayer.displayName}");
            Networking.LocalPlayer.TeleportTo(trSpawn.position, Quaternion.identity);
        }
    }

    public void ChangeClipboardMaterials(int id) {
        RoleManager role = privateManager.manager.roleManager;
        //role.체크하는 함수
        int typeid = role.CheckRole(id);
        if (typeid == -1) {
            Debug.Log($"PrivateRoom, CheckClipboardMaterials, typeid : -1");
            return;
        }
        switch(typeid) {
            case (int)ROLE.HITLER:
                clipboardRole.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChangeRoleHitler");
                clipboardMember.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChangeMemberFascist");
                break;
            case (int)ROLE.FASCIST:
                clipboardRole.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, funcChangeFascist[privateManager.curFascist++]);
                clipboardMember.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChangeMemberFascist");
                break;
            case (int)ROLE.LIBERAL:
                Debug.Log(privateManager.curLiberal);
                clipboardRole.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, funcChangeLiberal[privateManager.curLiberal++]);
                clipboardMember.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChangeMemberLiberal");
                break;
        }
    }
    private void Start() {
        RoleManager roleManager = privateManager.manager.roleManager;
        funcChangeFascist = new string[roleManager.matRoleFascist.Length];
        funcChangeLiberal = new string[roleManager.matRoleLiberal.Length];
        for (int i = 0; i < roleManager.matRoleFascist.Length; ++i) {
            funcChangeFascist[i] = $"ChangeRoleFascist_{i}";
        }
        for (int i = 0; i < roleManager.matRoleLiberal.Length; ++i) {
            funcChangeLiberal[i] = $"ChangeRoleLiberal_{i}";
        }
    }
}
