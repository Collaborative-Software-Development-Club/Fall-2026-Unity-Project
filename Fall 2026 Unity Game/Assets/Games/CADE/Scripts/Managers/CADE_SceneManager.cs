using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CADE_SceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private string MenuName;

    public void ChangeScene(int scene)
    {
        if (scene != -1)
        {
            SceneManager.LoadScene(Enum.GetName(typeof(CADE_SceneList.Scenes), scene));
        } else
        {
            SceneManager.LoadScene(MenuName);
        }
    }
}
