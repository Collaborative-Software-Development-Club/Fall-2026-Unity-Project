using UnityEngine;

namespace BindingTime {
public class Player : MonoBehaviour {
    private int tileX, tileY;

    void Awake() {
        UIManager.player = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        transform.localPosition = new(tileX, tileY, transform.localPosition.z);
    }

    public void Move(int x, int y) {
        tileX += x;
        tileY += y;
    }
}
}