using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CADE_DeathZoneManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> zones;

    [SerializeField]
    private bool isSimultaneous;

    [SerializeField]
    private bool isAlternating;

    [SerializeField]
    private float stagger;

    [SerializeField]
    private float cooldown;

    [SerializeField]
    private float alternationSize;

    private bool toggle = false;
    private bool completionCheck = false;
    private int count = 0;

    // Update is called once per frame
    void Update()
    {

        if (toggle == completionCheck)
        {
            completionCheck = !completionCheck;

            if (isAlternating)
            {   
                StartCoroutine(Alternation());
            }
            else
            {
                StartCoroutine(StaggeredZones());
            }
        }
    } 

    IEnumerator StaggeredZones()
    {

        foreach (GameObject zone in zones)
        {
            yield return new WaitForSeconds(stagger);
            zone.SetActive(toggle);
        }

        toggle = !toggle;
        completionCheck = toggle;
    }

    IEnumerator Alternation()
    {

        for (int i = 0; i < zones.Count; i++)
        {
            if ((i + count) % alternationSize == 0) { zones[i].SetActive(false); }
        }

        yield return new WaitForSeconds(cooldown);

        foreach (GameObject zone in zones) { zone.SetActive(true); }

        count++;
        completionCheck = toggle;
    }
}
