using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class CADE_DeathZone : MonoBehaviour
{
    [SerializeField] public UnityEvent OnDeath;

    private void Awake()
    {
        OnDeath ??= new UnityEvent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody == null)
            return;

        if (!collision.attachedRigidbody.CompareTag("Player"))
            return;

        Debug.Log("Death zone triggered by player!");

        OnDeath.Invoke();
        CADE_DeathCounter.RegisterDeath();
    }
}
