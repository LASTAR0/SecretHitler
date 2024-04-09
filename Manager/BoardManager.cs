
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BoardManager : UdonSharpBehaviour
{
    public GameManager manager;
    public Board[] boards;
    
    private int _count = 0;
    private int _max = 0;
    private int _voteYes = 0;
    private int _voteNo = 0;
    

    public void SetBoard() {
        int max = boards.Length;
        Board[] tb = new Board[max];
        Array.Copy(boards, tb, max);
        
        for (int i = max - 1; i > 0; i--) {
            int rand = UnityEngine.Random.Range(0, i + 1);
            Board temp = tb[i];
            tb[i] = tb[rand];
            tb[rand] = temp;
        }

        int[] pool = manager.playerManager.playerPool;
        for (int i = 0; i < pool.Length; ++i) {
            tb[i].id = pool[i];
            tb[i].RequestSerialization();
        }
    }

    // [Master]
    public void ActiveVote() {
        if (!Networking.IsMaster) return;
        _max = 0;
        foreach(Board b in boards) {
            if (b.id > 0) {
                _max++;
                b.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShowVote");
                b.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EnableImage");
            }
        }
    }

    // [Master] Check Vote
    public void CheckVoteYes() { _voteYes++; CheckVote(); }
    public void CheckVoteNo() { _voteNo++; CheckVote(); }
    // [Master] Check All Player Voted
    public void CheckVote() {
        _count++;
        // All Player Voting
        if (_count >= _max) {
            _count = 0;
            _max = 0;
            foreach (Board b in boards) {
                if (b.id > 0) { b.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChangeVoteAndDelay"); }
            }
            
            if (_voteYes > _voteNo) {  
                manager.tagManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EquipChancellor");
            } else { manager.tagManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "CancelChancellor"); }
            _voteYes = 0;
            _voteNo = 0;
        }
    }

    // [Master] after game ending, show all role
    public void ShowRoleAll() {
        foreach(Board b in boards) {
            if (b.id > 0) {
                b.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShowRoleAll");
            }
        }
    }
}
