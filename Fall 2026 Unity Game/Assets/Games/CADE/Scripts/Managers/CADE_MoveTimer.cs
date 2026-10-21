using System;
using UnityEngine;
using UnityEngine.Events;

public class CADE_MoveTimer : MonoBehaviour {
    [Tooltip("Seconds")] [SerializeField] private float moveTime = 2f;
    [SerializeField] private UnityEvent OnDeath;
    [SerializeField] private Rigidbody2D player;

    private float moveTimer;
    
    private void Start() {
        moveTimer = moveTime;
        OnDeath ??= new UnityEvent();
    }

    private void Update() {
        if (player.linearVelocity.sqrMagnitude < (0.5*0.5)) {
            moveTimer -= Time.deltaTime;
        } else {
            moveTimer = moveTime;
        }

        if (moveTimer <= 0) {
            OnDeath.Invoke();
        }
    }
}
