using UnityEngine;

public class CircularRotation : MonoBehaviour
{
    [SerializeField] private float rotSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * rotSpeed);
    }
}
