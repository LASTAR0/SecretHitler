
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TagManager : UdonSharpBehaviour
{
    public GameManager manager;
    public Nametag tagHitler;
    public Nametag[] tagFascist;
    public President tagPresident;
    public Chancellor tagChancellor;
    public Badge badgePresident;
    public Badge badgeChancellor;

    private int _savedId = -1;

    // send custom all
    public void OnEnableRoleTags() {
        Player player = manager.player;
        RoleManager role = manager.roleManager;
        int localId = Networking.LocalPlayer.playerId;
        int pnum = manager.playerManager.playerPool.Length;
        
        if (pnum > 6) {
            if (player.roleType == (int)ROLE.FASCIST) {
                int cur = 0;
                tagHitler.OnEnableMark(role.Hitler);
                for (int i = 0; i < role.Fascist.Length; ++i) {
                    if (localId != role.Fascist[i] && role.Fascist[i] != -1) {
                        tagFascist[cur++].OnEnableMark(role.Fascist[i]);
                    }
                }
            }
        } else {
            if (player.roleType == (int)ROLE.FASCIST) {
                tagHitler.OnEnableMark(role.Hitler);
            } else if (player.roleType == (int)ROLE.HITLER) {
                tagFascist[0].OnEnableMark(role.Fascist[0]);
            }
        }
    }
    public void OnEnableCandidateTags() {
        
    }
    public void EquipPresident() {
        int id = Networking.GetOwner(badgePresident.gameObject).playerId;
        tagPresident.EnablePresident(id);
    }

    // [All] after vote, equip tag
    public void EquipChancellor() {
        tagChancellor.EnableChancellor(_savedId);
        if (Networking.LocalPlayer.playerId == _savedId) {
            manager.player.candidate = (int)CANDIDATE.CHANCELLOR;
        }
        // Check Hitler (Victory Condition)
        if (Networking.LocalPlayer.isMaster) {
            CheckHitler();
        }
    }
    // [Master]
    public void CheckHitler() {
        if (manager.roleManager.CheckRole(_savedId) == (int)ROLE.HITLER && manager.briefingManager.numFascist >= 3) {
            manager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "FascistVictory");
        }
    }

    // [All] equip tag cancel
    public void CancelChancellor() {
        _savedId = -1;
        badgeChancellor.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OnEnableBadge");
        badgeChancellor.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OnResetPos");
    }

    // [All] Start Vote and Disable Chancellor
    public void StartVoteChancellor() {
        _savedId = Networking.GetOwner(badgeChancellor.gameObject).playerId;
        tagChancellor.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Disable");
    }
}
