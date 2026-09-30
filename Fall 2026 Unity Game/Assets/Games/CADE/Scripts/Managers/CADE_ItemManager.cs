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

    public void ResetAllFlags()
    {
        standardJumpFlag = false;
        doubleJumpFlag = false;
        wallJumpFlag = false;
        dashFlag = false;
        stickyFlag = false;
    }

    // Callback function invoked by item grabbing event
    public void OnGetItem(int itemId)
    {
        if (itemId == (int)CADE_ItemList.Items.DoubleJump) { OnGetDoubleJump(); } 
        else if (itemId == (int) CADE_ItemList.Items.WallJump) {  OnGetWallJump(); } 
        else if (itemId == (int) CADE_ItemList.Items.Dash) { OnGetDash(); }
        else if (itemId == (int) CADE_ItemList.Items.Sticky) { OnGetSticky(); }
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
}
