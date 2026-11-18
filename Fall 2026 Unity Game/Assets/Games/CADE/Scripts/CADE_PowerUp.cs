using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class CADE_PowerUp : I_Item
{
    public CADE_ItemList.Items m_Item;

    private bool isInverted = false;
    private int invertedRuleId;

    public UnityEvent<int> InvertedItemRuleDiscovery;

    [SerializeField]
    private UnityEvent<int> OnGetItem;

    private void Start()
    {
        // give dummy value just in case
        invertedRuleId = 0;

        OnGetItem ??= new UnityEvent<int>();
        InvertedItemRuleDiscovery ??= new UnityEvent<int>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered");

        if (collision.attachedRigidbody.CompareTag("Player"))
        {
            OnGetItem.Invoke((int)m_Item);
            if (isInverted)
            {
                InvertedItemRuleDiscovery.Invoke(invertedRuleId);
            }
            Debug.Log("Grabbed Item: " + Enum.GetName(typeof(CADE_ItemList.Items), m_Item));
            gameObject.SetActive(false);
        }
    }

    public void InvertItem()
    {
        isInverted = true;

        if (m_Item == CADE_ItemList.Items.DoubleJump)
        {
            m_Item = CADE_ItemList.Items.Inv_DoubleJump;
            invertedRuleId = (int)CADE_RandomRulesList.DoubleJumpRules.PowerDowns;
        }
        else if (m_Item == CADE_ItemList.Items.WallJump)
        {
            m_Item = CADE_ItemList.Items.Inv_WallJump;
            invertedRuleId = (int)CADE_RandomRulesList.WallJumpRules.PowerDowns;
        }
        else if (m_Item == CADE_ItemList.Items.Dash)
        {
            m_Item = CADE_ItemList.Items.Inv_Dash;
            invertedRuleId = (int)CADE_RandomRulesList.DashRules.PowerDowns;
        }
        else if (m_Item == CADE_ItemList.Items.Sticky)
        {
            m_Item = CADE_ItemList.Items.Inv_Sticky;
            invertedRuleId = (int)CADE_RandomRulesList.StickyRules.PowerDowns;
        }
    }
}
