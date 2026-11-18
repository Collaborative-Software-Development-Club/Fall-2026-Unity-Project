using UnityEngine;

public class CADE_Notification : MonoBehaviour
{
    public GameObject notificationIcon;

    void Start()
    {
        // will add fade-in and out here
    }

    public void ShowNotification()
    {
        notificationIcon.SetActive(true);
    }

    public void HideNotification()
    {
        notificationIcon.SetActive(false);
    }
}
