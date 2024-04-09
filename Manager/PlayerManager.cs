using UnityEngine.UI;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

public class PlayerManager : UdonSharpBehaviour
{
    public GameManager manager;
    [UdonSynced] public int[] players;
    [UdonSynced] public int[] playerPool;

    [UdonSynced, HideInInspector] public int cntPlayers;
    private const int MAX_PLAYER = 10;

    public int GetPlayerCount() {
        int cnt = 0;
        foreach (int id in players) {
            if (id != -1) cnt++;
        }
        return cnt;
    }
    public void SetRandomShuffle() {
        int cnt = GetPlayerCount();
        int cur = 0;
        playerPool = new int[cnt];

        for (int i = 0; i < players.Length; ++i) {
            if (players[i] != -1) playerPool[cur++] = players[i];
        }

        int n = playerPool.Length;
        for (int i = n - 1; i > 0; i--) {
            int rand = UnityEngine.Random.Range(0, i + 1);
            int temp = playerPool[i];
            playerPool[i] = playerPool[rand];
            playerPool[rand] = temp;
        }

        RequestSerialization();
    }
    public void addPlayer(VRCPlayerApi p) {
        int index = getEmptyPlayers();
        if (index == -1) {
            Debug.Log($"index is {index})");
            return;
        }
        players[index] = p.playerId;
        cntPlayers++;
        RequestSerialization();
    }
    public void subPlayer(VRCPlayerApi p) {
        int index = getPlayer(p);
        if (index == -1) {
            Debug.Log($"index is {index}");
            return;
        }
        players[index] = -1;
        cntPlayers--;
        RequestSerialization();
    }
    public int getEmptyPlayers() {
        for (int i = 0; i < players.Length; ++i) {
            if (players[i] == -1) {
                Debug.Log($"{i}번째가 비었다");
                return i;
            }
        }
        return -1;
    }
    public int getPlayer(VRCPlayerApi p) {
        for (int i = 0; i< players.Length; ++i) {
            if (players[i] == p.playerId) {
                return i;
            }
        }
        return -1;
    }
    public void updateText() {
        string names = "";
        foreach (int id in manager.playerManager.players) {
            if (id != -1) {
                names += VRCPlayerApi.GetPlayerById(id).displayName + "\n";
            }
        }
    }
    private void resetPlayers() {
        for (int i = 0; i < players.Length; ++i)
            players[i] = -1;
    }

    private void Start() {
        players = new int[MAX_PLAYER];
        resetPlayers();
    }
    public override void OnPreSerialization() {
        if (!manager.isPlaying) {
            manager.lobbyUI.RequestSerialization();
        }
    }
}
