using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CADE_RuleUIManager : MonoBehaviour
{
    private Dictionary<int, string> AllRules = new Dictionary<int, string>();
    private Dictionary<int, string> DiscoveredRules;
    private Dictionary<int, string> SeenRules;
    private bool rulesPanelOpen = false;

    [SerializeField]
    private TMP_Text rulesText;

    [SerializeField]
    private GameObject rulesPanel;
    
    [SerializeField]
    private CADE_Notification notificationIcon;

    public void NewRuleAdded(int ruleId, string ruleDesc)
    {

        AllRules.Add(ruleId, ruleDesc);
        Debug.Log("Successfully Added rule: " + ruleId);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rulesPanelOpen = rulesPanel.gameObject.activeSelf;

        notificationIcon.HideNotification();

        //On game start, the player has not discovered or looked at any rules

        DiscoveredRules = new Dictionary<int, string>();
        SeenRules = new Dictionary<int, string>();
        rulesText.text = string.Empty;
    }

    // Update is called once per frame
    public void OnDiscoverNewRule(int ruleId)
    {
        // Only do if rule is not already there
        if (!DiscoveredRules.ContainsKey(ruleId))
        {
            DiscoveredRules.Add(ruleId, AllRules[ruleId]);

            if ((DiscoveredRules.Count > SeenRules.Count) && !rulesPanel.gameObject.activeSelf)
            {
                notificationIcon.ShowNotification();
            }

            rulesText.text = rulesText.text + DiscoveredRules[ruleId];
        }
    }

    // Update is called once per frame
    public void ToggleOpenRulesList()
    {
        if (!rulesPanelOpen)
        {
            notificationIcon.HideNotification();

            SeenRules.Clear();

            foreach (var rule in DiscoveredRules)
            {
                SeenRules.Add(rule.Key, rule.Value);
            }
        }

        rulesPanelOpen = !rulesPanelOpen;
    }
}
