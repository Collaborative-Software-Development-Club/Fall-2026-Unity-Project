using System.Collections;
using UnityEngine;

public abstract class I_Item : MonoBehaviour {
    public float dur { get; set; } = 5;
    
    IEnumerator effect() {
        Debug.Log("Do something");
        yield return new WaitForSeconds(dur);
        Debug.Log("Stop do something");
    }
    void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            StartCoroutine(effect());
        }
    }
}
