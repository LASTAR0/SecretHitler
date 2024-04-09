using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public enum CANDIDATE {
    NOTHING = 0, PRESIDENT = 1, CHANCELLOR = 2
}
public enum MEMBER {
    FASCIST = 0, LIBERAL = 1
}
enum ROLE {
    NOTHING = 0,
    HITLER = 1,
    FASCIST = 2,
    LIBERAL = 3
}
public enum POLICY {
    FASCIST,
    LIBERAL
}
public class GameManager : UdonSharpBehaviour
{
    public PlayerManager playerManager;
    public PrivateManager privateManager;
    public RoleManager roleManager;
    public TagManager tagManager;
    public BoardManager boardManager;
    public PolicyManager policyManager;
    public BriefingManager briefingManager;
    public TrackerManager trackerManager;
    public SoundManager soundManager;
    public PropManager propManager;
    public LobbyUI lobbyUI;
    public Player player;

    [UdonSynced] public bool isPlaying = false;
    [HideInInspector] public bool TEST = false;
    private bool _isSynced = false;

    public void Sync() {
        lobbyUI.masterName.text = Networking.GetOwner(gameObject).displayName;
    }
    public void OnPlay() {
        if (isPlaying) return;

        isPlaying = true;
        RequestSerialization();
        
        playerManager.SetRandomShuffle();
        roleManager.SetRole(playerManager.playerPool);
        player.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetRole");
        privateManager.JoinPlayer(playerManager.playerPool);
        tagManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OnEnableRoleTags");
        boardManager.SendCustomEventDelayedSeconds("SetBoard", 1.0f);
        policyManager.SendCustomEventDelayedSeconds("ReadyDeck", 2.0f);
        briefingManager.SendCustomEventDelayedSeconds("ChangeBriefingMaterial", 3.0f);
        soundManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChangeTheme");
    }
    // [Local] End Game
    public void FascistVictory() {
        if (player.roleType == (int)ROLE.HITLER || player.roleType == (int)ROLE.FASCIST) {
            soundManager.PlayVictory();
        } else {
            soundManager.PlayDefeat();
        }
        if (Networking.LocalPlayer.isMaster)
            boardManager.ShowRoleAll();
    }
    public void LiberalVictory() {
        if (player.roleType == (int)ROLE.FASCIST || player.roleType == (int)ROLE.FASCIST) {
            soundManager.PlayDefeat();
        } else {
            soundManager.PlayVictory();
        }
        if (Networking.LocalPlayer.isMaster)
            boardManager.ShowRoleAll();
    }

    public override void OnPlayerJoined(VRCPlayerApi player) {
        if (Networking.LocalPlayer.isMaster) {
            RequestSerialization();
            if (!_isSynced) {
                _isSynced = true;
                lobbyUI.masterName.text = Networking.GetOwner(gameObject).displayName;
            }
        }

    }
    public override void OnPlayerLeft(VRCPlayerApi player) {
        
    }
    public override void OnDeserialization() {
        if (!_isSynced) {
            _isSynced = true;
            Sync();
        }
    }
}
