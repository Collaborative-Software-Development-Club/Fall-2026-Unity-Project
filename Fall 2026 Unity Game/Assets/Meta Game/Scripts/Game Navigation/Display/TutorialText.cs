using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class TutorialText : MonoBehaviour
{
    public void Vanish(CallbackContext context)
    {
        gameObject.SetActive(false);

    }
}
