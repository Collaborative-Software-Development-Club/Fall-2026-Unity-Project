using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class CADE_LaunchBush : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D playerRb;

    [SerializeField] 
    private float force;

    private bool inBush = false;

    public void LaunchPlayer()
    {
        if (inBush)
        {
            playerRb.AddForceY(force);
            Debug.Log("Launched");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inBush = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inBush = false;
    }
}
