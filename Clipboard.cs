
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Clipboard : UdonSharpBehaviour
{
    public RoleManager roleManager;
    public PrivateRoom privateRoom;
    public MeshRenderer matImage;

    public void ChangeRoleHitler() {
        matImage.material = roleManager.matRoleHitler;
    }
    public void ChangeRoleFascist_0() { matImage.material = roleManager.matRoleFascist[0]; }
    public void ChangeRoleFascist_1() { matImage.material = roleManager.matRoleFascist[1]; }
    public void ChangeRoleFascist_2() { matImage.material = roleManager.matRoleFascist[2]; }
    public void ChangeRoleLiberal_0() { matImage.material = roleManager.matRoleLiberal[0]; }
    public void ChangeRoleLiberal_1() { matImage.material = roleManager.matRoleLiberal[1]; }
    public void ChangeRoleLiberal_2() { matImage.material = roleManager.matRoleLiberal[2]; }
    public void ChangeRoleLiberal_3() { matImage.material = roleManager.matRoleLiberal[3]; }
    public void ChangeRoleLiberal_4() { matImage.material = roleManager.matRoleLiberal[4]; }
    public void ChangeRoleLiberal_5() { matImage.material = roleManager.matRoleLiberal[5]; }
    public void ChangeMemberFascist() { matImage.material = roleManager.matMemberFascist; }
    public void ChangeMemberLiberal() { matImage.material = roleManager.matMemberLiberal; }
}
