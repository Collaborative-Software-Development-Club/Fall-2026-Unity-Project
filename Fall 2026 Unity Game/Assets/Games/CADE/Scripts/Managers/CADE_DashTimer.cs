using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CADE_DashTimer : MonoBehaviour {
    [Tooltip("In seconds")] [SerializeField] private float time;
    
    [SerializeField] private TMP_Text timerText;
    
    [SerializeField] private UnityEvent OnDeath;

    private float timer;

    private void Start() {
        timer = time;
        OnDeath ??= new UnityEvent();
    }

    private void Update() {
        timer -= Time.deltaTime;
        timerText.text = Mathf.Round(timer).ToString();

        if (timer <= 0) {
            OnDeath.Invoke();
        }
    }
}
