using UnityEngine;

namespace BindingTime {
    public class GameManager : MonoBehaviour {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] private GameObject player;
        
        void Start() {
            
        }

        // Update is called once per frame
        void Update() {
            UIManager.Update();
        }
    }
}