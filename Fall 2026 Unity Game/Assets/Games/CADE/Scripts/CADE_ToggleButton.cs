using UnityEngine;

public class CADE_ToggleButton : MonoBehaviour
{
    public GameObject object1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAndOff()
    {
        if (!object1.activeInHierarchy)
        {
            object1.SetActive(true);
        }
        else
        {
            object1.SetActive(false);
        }
    }
}
