using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CADE_DashRules : MonoBehaviour
{
    [Tooltip("In seconds")] [SerializeField] private float maxTime = 50f;
    [Tooltip("Seconds")] [SerializeField] private float moveTime = 2f;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private UnityEvent OnDeath;
    [SerializeField] private Rigidbody2D player;
    
    private float timer;
    private float moveTimer;
    
    private void Start() {
        timer = maxTime;
        moveTimer = moveTime;
        OnDeath ??= new UnityEvent();
    }

    private void Update() {
        timer -= Time.deltaTime;
        timerText.text = Mathf.Round(timer).ToString();
            
        if (player.linearVelocity.sqrMagnitude < (0.5*0.5)) {
            moveTimer -= Time.deltaTime;
        } else {
            moveTimer = moveTime;
        }
        
        if (timer <= 0 || moveTimer <= 0) {
            OnDeath.Invoke();
        }

    }
}