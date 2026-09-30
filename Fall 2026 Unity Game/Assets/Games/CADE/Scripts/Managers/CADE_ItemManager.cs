using UnityEngine;
using UnityEngine.InputSystem;

public class CADE_ItemManager : MonoBehaviour
{
    // Jump Effects
    public bool standardJumpFlag = false;
    public bool doubleJumpFlag = false;
    public bool wallJumpFlag = false;

    // Dash Effect
    public bool dashFlag = false;

    // Sticky Effect
    public bool stickyFlag = false;
    
    // Inverse Effects
    public bool inv_DoubleJumpFlag = false;
    public bool inv_wallJumpFlag = false;
    public bool inv_DashFlag = false;
    public bool inv_StickyFlag = false;
    
    public void ResetAllFlags()
    {
        standardJumpFlag = false;
        doubleJumpFlag = false;
        wallJumpFlag = false;
        dashFlag = false;
        stickyFlag = false;
        inv_DoubleJumpFlag = false;
        inv_wallJumpFlag = false;
        inv_DashFlag = false;
        inv_StickyFlag = false;
    }

    // Callback function invoked by item grabbing event
    public void OnGetItem(int itemId)
    {
        if (itemId == (int)CADE_ItemList.Items.DoubleJump) { OnGetDoubleJump(); } 
        else if (itemId == (int) CADE_ItemList.Items.WallJump) {  OnGetWallJump(); } 
        else if (itemId == (int) CADE_ItemList.Items.Dash) { OnGetDash(); }
        else if (itemId == (int) CADE_ItemList.Items.Sticky) { OnGetSticky(); }
        else if (itemId == (int) CADE_ItemList.Items.Inv_DoubleJump) { OnInvDoubleJump(); }
        else if (itemId == (int) CADE_ItemList.Items.Inv_WallJump) { OnInvWallJump(); }
        else if (itemId == (int) CADE_ItemList.Items.Inv_Dash) { OnInvDash(); }
        else if (itemId == (int) CADE_ItemList.Items.Inv_Sticky) { OnInvSticky(); }
    }

    public void OnGetDoubleJump()
    {
        ResetAllFlags();
        doubleJumpFlag = true;
    }

    public void OnGetWallJump()
    {
        ResetAllFlags();
        wallJumpFlag = true;
    }

    public void OnGetDash()
    {
        ResetAllFlags();
        dashFlag = true;
    }

    public void OnGetSticky()
    {
        ResetAllFlags();
        stickyFlag = true;
    }

    public void OnInvDoubleJump() {
        ResetAllFlags();
        inv_DoubleJumpFlag = true;
    }

    public void OnInvWallJump() {
        ResetAllFlags();
        inv_wallJumpFlag = true;
    }

    public void OnInvDash() {
        ResetAllFlags();
        inv_DashFlag = true;
    }

    public void OnInvSticky() {
        ResetAllFlags();
        inv_StickyFlag = true;
    }
}
