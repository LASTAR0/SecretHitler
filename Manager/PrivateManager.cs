
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PrivateManager : UdonSharpBehaviour
{
    public GameManager manager;
    public PrivateRoom[] privateRooms;
    [HideInInspector] public int curFascist = 0;
    [HideInInspector] public int curLiberal = 0;

    public void JoinPlayer(int[] pool) {
        int cur = 0;
        foreach(int id in pool) {
            VRCPlayerApi p = VRCPlayerApi.GetPlayerById(id);
            if (!privateRooms[cur].isJoined) {
                privateRooms[cur++].JoinPlayer(id);
            }
        }
    }
    public void OnReset() {
        curFascist = 0;
        curLiberal = 0;
    }
}