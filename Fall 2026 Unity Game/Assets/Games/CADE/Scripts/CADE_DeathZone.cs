using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Events;

public class CADE_DeathZone : MonoBehaviour
{
    [SerializeField] private UnityEvent OnDeath;

    private void Start()
    {
        OnDeath ??= new UnityEvent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Player"))
        {
            OnDeath.Invoke();
        }
    }
}
