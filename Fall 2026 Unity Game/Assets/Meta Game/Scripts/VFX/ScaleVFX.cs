using System.Collections;
using UnityEngine;

public class ScaleVFX : MonoBehaviour
{
    private void Start()
    {
        ChangeScale(new Vector2(5, 2), 5);
    }
    public void ChangeScale(Vector2 scale, float time)
    {
        float currTime = 0f;
        while (currTime < 1)
        {
            gameObject.transform.localScale = Vector2.Lerp(gameObject.transform.localScale, scale, currTime);
            currTime += Time.deltaTime / time;
            Debug.Log(currTime);
        }
       
    }
}
