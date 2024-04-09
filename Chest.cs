
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Chest : UdonSharpBehaviour
{    
    public GameManager manager;
    public ChestBox chestBox;
    public CheckIcon checkIcon;
    private VRCPlayerApi _player;

    private const float FadeDuration = 3.0f;
    private bool _isEntering = false;
    private bool _isEquip = false;
    private float _fill = 0.0f;
    private int _tagType = 0;
    private void Update() {
        Vector3 pos = _player.GetBonePosition(HumanBodyBones.LeftHand);
        Quaternion rot = _player.GetBoneRotation(HumanBodyBones.LeftHand);
        transform.position = pos;
        transform.localRotation = rot;
        
        if (!_isEntering) return;

        _fill += Time.deltaTime / FadeDuration;
        _fill = Mathf.Clamp01(_fill);
        checkIcon.image.fillAmount = _fill;
        if (checkIcon.image.fillAmount >= 1.0f) {
            ResetIcon();
            if (_isEquip) {
                chestBox.Attacth();
                switch (_tagType) {
                    case (int)CANDIDATE.PRESIDENT:
                        manager.tagManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EquipPresident");
                        manager.player.candidate = (int)CANDIDATE.PRESIDENT;
                        break;
                    case (int)CANDIDATE.CHANCELLOR:
                        manager.tagManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "StartVoteChancellor");
                        manager.boardManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ActiveVote");
                        break;
                }
            } else {
                int candidate = manager.player.candidate;
                switch (candidate) {
                    case (int)CANDIDATE.PRESIDENT:
                        manager.tagManager.badgePresident.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OnEnableBadge");
                        manager.tagManager.badgePresident.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OnResetPos");
                        manager.tagManager.tagPresident.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Disable");
                        break;
                    case (int)CANDIDATE.CHANCELLOR:
                        manager.tagManager.badgeChancellor.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OnEnableBadge");
                        manager.tagManager.badgeChancellor.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OnResetPos");
                        manager.tagManager.tagChancellor.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Disable");
                        break;
                }
                manager.player.candidate = (int)CANDIDATE.NOTHING;
            }
        }
    }
    public void EquipTag(int tagType) {
        ResetIcon();
        _isEntering = true;
        _isEquip = true;
        _tagType = tagType;
        
    }
    public void UnEquipTag() {
        ResetIcon();
        _isEntering = true;
        _isEquip = false;
    }
    public void ResetIcon() {
        _isEntering = false;
        _fill = 0.0f;
        checkIcon.image.fillAmount = _fill;
    }
    private void Start()
    {
        _player = Networking.LocalPlayer;
    }
}
