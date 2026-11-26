using System.Collections;
using UnityEngine;

public class SmoothScale : MonoBehaviour
{
    public void ChangeScale(Vector3 scale, float time)
    {
        StartCoroutine(LerpScale(scale, time));
    }

    IEnumerator LerpScale(Vector3 scale, float time)
    {
        float currTime = 0f;
        Vector2 originalScale = gameObject.transform.localScale;
        while (currTime < 1)
        {
            gameObject.transform.localScale = Vector3.Lerp(originalScale, scale, currTime);
            currTime += Time.deltaTime / time;
            yield return new WaitForEndOfFrame();
        }
       
    }
}
