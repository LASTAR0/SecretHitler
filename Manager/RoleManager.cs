
using UdonSharp;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


public class RoleManager : UdonSharpBehaviour
{
    public GameManager manager;
    [UdonSynced] public int Hitler;
    [UdonSynced] public int[] Fascist;
    [UdonSynced] public int[] Liberal;

    public Material matRoleHitler;
    public Material[] matRoleFascist;
    public Material[] matRoleLiberal;
    public Material matMemberFascist;
    public Material matMemberLiberal;
    
    private const int MAX_FASCIST = 3;
    private const int MAX_LIBERAL = 6;

    // 5명: 자유당원 3장 + 파시스트 1장 + 히틀러 1장
    // 6명: 자유당원 4장 + 파시스트 1장 + 히틀러 1장
    // 7명: 자유당원 4장 + 파시스트 2장 + 히틀러 1장
    // 8명: 자유당원 5장 + 파시스트 2장 + 히틀러 1장
    // 9명: 자유당원 5장 + 파시스트 3장 + 히틀러 1장
    // 10명: 자유당원 6장 + 파시스트 3장 + 히틀러 1장
    public void SetRole(int[] pool) {
        int cnt = pool.Length;
        int cur = 0;

        //Hitler = pool[0]; Fascist[0] = pool[1]; Liberal[0] = pool[2]; RequestSerialization(); return;

        switch (cnt) {
            case 5:
                Fascist = new int[1];
                Liberal = new int[3];
                break;
            case 6:
                Fascist = new int[1];
                Liberal = new int[4];
                break;
            case 7:
                Fascist = new int[2];
                Liberal = new int[4];
                break;
            case 8:
                Fascist = new int[2];
                Liberal = new int[5];
                break;
            case 9:
                Fascist = new int[3];
                Liberal = new int[5];
                break;
            case 10:
                Fascist = new int[3];
                Liberal = new int[6];
                break;
        }

        Hitler = pool[cur++];
        
        for (int i = 0; i < Fascist.Length; ++i) {
            Fascist[i] = pool[cur++];
        }
        for (int i = 0; i < Liberal.Length; ++i) {
            Liberal[i] = pool[cur++];
        }
        RequestSerialization();
    }
    // check role
    // CheckRole(player id) -> 1 = hitler, 2 = fascist, 3 = liberal
    public int CheckRole(int  id) {
        if (Hitler == id) { return (int)ROLE.HITLER; }
        foreach (int num in Fascist) {
            if (num == id) {
                return (int)ROLE.FASCIST;
            }
        }
        foreach (int num in Liberal) {
            if (num == id) {
                return (int)ROLE.LIBERAL;
            }
        }
        return -1;
    }
    private int GetEmptyFascist() {
        for(int i = 0; i < Fascist.Length; ++i) {
            if (Fascist[i] == -1) {
                return i;
            }
        }
        Debug.Log("Can't Find Empty Fascist Slot !");
        return -1;
    }

    public override void OnPreSerialization() {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "UpdateText");
    }
    private void Reset() {
        for (int i = 0; i < Fascist.Length; ++i) {
            Fascist[i] = -1;
        }
        for (int i = 0; i < Liberal.Length; ++i) {
            Liberal[i] = -1;
        }
    }
    private void Start() {
        Fascist = new int[MAX_FASCIST];
        Liberal = new int[MAX_LIBERAL];
        Reset();
    }
}
