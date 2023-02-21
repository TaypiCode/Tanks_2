using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModuleInTankPanelUI : MonoBehaviour
{
    [SerializeField] private Image _moduleUIImg;
    [SerializeField] private TextMeshProUGUI _repairText;
    [SerializeField] private Color _orangeColor;
    [SerializeField] private Color _redColor;
    public void SetRepairTime(float val)
    {
        string text = "";
        if(val > 0)
        {
            text = Mathf.CeilToInt(val).ToString();
        }
        _repairText.text = text;
    }
    public void SetRedColor()
    {
        _moduleUIImg.color = _redColor;
    }
    public void SetOrangeColor()
    {
        _moduleUIImg.color = _orangeColor;
    }
    public void SetWhiteColor()
    {
        _moduleUIImg.color = Color.white;
    }
}
