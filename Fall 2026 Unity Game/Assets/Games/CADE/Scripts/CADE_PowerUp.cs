using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CADE_PowerUp : I_Item
{
    [SerializeField]
    private CADE_ItemList.Items m_Item;

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
            Debug.Log("Grabbed Item");
            Destroy(gameObject);
        }
    }
}
