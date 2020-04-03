using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private Text tooltipText;
    void Start()
    {
        tooltipText = GetComponentInChildren<Text>();
        gameObject.SetActive(false);
    }

    public void GenerateTooltip(GameItem item)
    {
        string statsText = "";
        if(item.stats.Count > 0)
        {
            statsText += "\n";
            foreach (var stat in item.stats)
            {
                statsText += stat.Key.ToString() + ": " + stat.Value + "\n";
            }
        }

        string wholeTooltip = string.Format("<b><color=#004225>{0}</color></b>\n{1}\n{2}", item.title, item.description, statsText);
        tooltipText.text = wholeTooltip;
        gameObject.SetActive(true);
    }
}
