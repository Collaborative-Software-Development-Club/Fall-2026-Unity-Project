using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CADE_PowerUp : I_Item
{
    public CADE_ItemList.Items m_Item;

    [SerializeField]
    private UnityEvent<int> OnGetItem;

    private void Start()
    {
        OnGetItem ??= new UnityEvent<int>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered");

        if (collision.attachedRigidbody.CompareTag("Player"))
        {
            OnGetItem.Invoke((int)m_Item);
            Debug.Log("Grabbed Item: " + Enum.GetName(typeof(CADE_ItemList.Items), m_Item));
            Destroy(gameObject);
        }
    }

    public void InvertItem()
    {
        if (m_Item == CADE_ItemList.Items.DoubleJump)
        {
            m_Item = CADE_ItemList.Items.Inv_DoubleJump;
        }
        else if (m_Item == CADE_ItemList.Items.WallJump)
        {
            m_Item = CADE_ItemList.Items.Inv_WallJump;
        }
        else if (m_Item == CADE_ItemList.Items.Dash)
        {
            m_Item = CADE_ItemList.Items.Inv_Dash;
        }
        else if (m_Item == CADE_ItemList.Items.Dash)
        {
            m_Item = CADE_ItemList.Items.Inv_Dash;
        }
    }
}
