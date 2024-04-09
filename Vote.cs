
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Vote : UdonSharpBehaviour
{
    public Board board;
    public Material[] matVote;
    public MeshRenderer mesh;
    // 0 = no, 1 = yes
    private int _voteType = -1;
    public override void Interact() {
        Debug.Log($"_voteType : {_voteType}");
        if (_voteType == 0) {
            board.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "VoteSelectedNo");
            board.boardManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "CheckVoteNo");
        } else if (_voteType == 1) {
            board.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "VoteSelectedYes");
            board.boardManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "CheckVoteYes");
        }
        board.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "CheckImage");
        
    }
    public void EnableVoteYes() {
        gameObject.SetActive(true);
        mesh.material = matVote[1];
        _voteType = 1;
    }
    public void EnableVoteNo() {
        gameObject.SetActive(true);
        mesh.material = matVote[0];
        _voteType = 0;
    }
    public void HideVote() {
        //mesh.material = null;
        //_voteType = -1;
        gameObject.SetActive(false);
    }
}
