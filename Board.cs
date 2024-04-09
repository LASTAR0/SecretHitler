
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class Board : UdonSharpBehaviour
{
    public BoardManager boardManager;
    [UdonSynced, HideInInspector] public int id = -1;
    public Transform trSpawn;
    public Image checkImg;
    public Image deadImg;
    [HideInInspector] public string displayName;
    public Text txtDisplayname;
    public Vote[] vote;
    [HideInInspector] public Color voteColor;
    [UdonSynced, HideInInspector] public bool _isDead = false;
    public GameObject[] objMember = new GameObject[3];
    public Text txtNotHitler;
    public ParticleGroup ptcgBullet;
    public ParticleGroup ptcgMagnifying;

    public override void OnPreSerialization() { Sync(); }
    public override void OnDeserialization() { Sync(); }

    // [All] Reset Function
    public void Reset() {
        id = -1;
        checkImg.enabled = false;
        deadImg.enabled = false;
        SetTextName();
        DisableVote();
        _isDead = false;
        txtNotHitler.enabled = false;
        DisableMemberText();
    }
    // RequestSerialization Function
    private void Sync() {
        SetTextName();
        if (_isDead) {
            deadImg.enabled = true;
            int roleid = boardManager.manager.roleManager.CheckRole(id);
            if (roleid == (int)ROLE.HITLER) { 
                boardManager.manager.LiberalVictory();
            } else if (roleid != (int)ROLE.HITLER) {
                txtNotHitler.enabled = true;
            }
        }
    }

    private void SetTextName() {
        if (_isDead) {
            txtDisplayname.text = "";
            return;
        }
        if (id == -1) {
            Debug.Log($"Board, OnDeserializetion id is {id}");
            txtDisplayname.text = "";
            return;
        }
        displayName = VRCPlayerApi.GetPlayerById(id).displayName;
        txtDisplayname.text = displayName;
    }

    // Board Owner Call
    public void ShowVote() {
        if (Networking.LocalPlayer.playerId != id) return;
        
        int rand = Random.Range(0, 9);
        if (rand < 5) {
            vote[0].EnableVoteNo();
            vote[1].EnableVoteYes();
        } else {
            vote[0].EnableVoteYes();
            vote[1].EnableVoteNo();
        }
    }
    public void VoteSelectedYes() {
        vote[0].gameObject.SetActive(false);
        vote[1].gameObject.SetActive(false);
        voteColor = Color.green;
    }
    public void VoteSelectedNo() {
        vote[0].gameObject.SetActive(false);
        vote[1].gameObject.SetActive(false);
        voteColor = Color.red;
    }
    // All Call
    public void ChangeVoteAndDelay() {
        txtDisplayname.color = voteColor;
        DisableImage();
        SendCustomEventDelayedSeconds("ResetVote", 7f);
    }
    public void ResetVote() {
        txtDisplayname.color = Color.white;
        checkImg.color = Color.grey;
    }
    public void DisableVote() {
        vote[0].HideVote();
        vote[1].HideVote();
    }
    
    // check Image
    public void EnableImage() {
        checkImg.enabled = true;
        checkImg.color = Color.grey;
    }
    public void DisableImage() {
        checkImg.enabled = false;
    }
    public void CheckImage() {
        checkImg.color = Color.green;
    }

    // about player survived
    // [All]
    public void KillPlayer() {
        _isDead = true;
        ptcgBullet.Active();
        if (Networking.LocalPlayer.isMaster) { RequestSerialization(); }
    }

    // about check membership
    // [local]
    public void EnableMemberText() {
        int roleid = boardManager.manager.roleManager.CheckRole(id);
        if (roleid == (int)ROLE.HITLER || roleid == (int)ROLE.FASCIST) {
            objMember[(int)ROLE.FASCIST - 1].SetActive(true);
        } else if (roleid == (int)ROLE.LIBERAL) {
            objMember[(int)ROLE.LIBERAL - 1].SetActive(true);
        } else {
            Debug.Log("Board : EnableMemberText : role id wrong");
        }
        ptcgMagnifying.Active();
        SendCustomEventDelayedSeconds("DisableMemberText", 7.0f);
    }
    public void DisableMemberText() {
        foreach (GameObject o in objMember) {
            o.SetActive(false);
        }
    }

    // [Local]
    public void ShowRoleAll() {
        int roleid = boardManager.manager.roleManager.CheckRole(id);
        if (roleid == (int)ROLE.HITLER) { objMember[(int)ROLE.HITLER - 1].SetActive(true); }
        else if (roleid == (int)ROLE.FASCIST) { objMember[(int)ROLE.FASCIST - 1].SetActive(true); }
        else if (roleid == (int)ROLE.LIBERAL) { objMember[(int)ROLE.LIBERAL - 1].SetActive(true); }
    }
}
