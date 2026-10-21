using System;
using UnityEngine;
using UnityEngine.Events;

public class CADE_DashWallTimer : MonoBehaviour {
    [Tooltip("Seconds")]
    [SerializeField] private float wallTouchTime;
    
    [SerializeField] private UnityEvent OnDeath;

    private float timer;
    private bool isTouchingWall;

    private void Start() {
        timer = wallTouchTime;
        OnDeath ??= new UnityEvent();
    }

    private void Update() {
        Debug.Log(timer);
        if (isTouchingWall) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                OnDeath.Invoke();
            }
        } else {
            timer = wallTouchTime;
        }
        
        isTouchingWall = false;
    }

    private void OnTriggerStay2D(Collider2D other) {
        isTouchingWall = true;
    }
}
