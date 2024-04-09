
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;



public class Player : UdonSharpBehaviour
{
    // all local
    public GameManager manager;
    // 0 = nothing
    // 1 = hitler
    // 2 = fascist
    // 3 = liberal
    public int roleType;
    public int id;
    // 0 = nothing
    // 1 = president
    // 2 = chancellor
    public int candidate;

    public void SetRoleTypeReset() { roleType = 0; }
    public void SetRoleTypeHitler() { roleType = 1; }
    public void SetRoleTypeFascist() { roleType = 2; }
    public void SetRoleTypeLiberal() { roleType = 3; }

    /*
     * @ SendCustom All
     */
    public void SetRole() {
        int id = Networking.LocalPlayer.playerId;
        RoleManager role = manager.roleManager;
        if (id == role.Hitler) {
            Debug.Log($"SetRole, you are hitler");
            roleType = (int)ROLE.HITLER; 
        }

        foreach (int num in role.Fascist) {
            if (id == num) {
                Debug.Log("SetRole, you are fascist");
                roleType = (int)ROLE.FASCIST;
            }
        }
        foreach (int num in role.Liberal) {
            if (id == num) {
                Debug.Log("SetRole, you are liberal");
                roleType = (int)ROLE.LIBERAL;
            }
        }
    }
    private void Start() {
        id = Networking.LocalPlayer.playerId;
    }
}
