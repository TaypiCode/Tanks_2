using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TankInfoInTeamUI : MonoBehaviour
{
    [SerializeField] private Image _aliveBg;
    [SerializeField] private Image _deadBg;

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _killCountText;

    private Tank _tank;

    public Tank Tank { get => _tank;  }

    public void SetTank(Tank tank)
    {
        _tank = tank;
        SetName(_tank.TankScriptable.TankName);
        if (_tank.IsAlive)
        {
            SetAlive();
        }
        else
        {
            SetDead();
        }
        UpdateKillCountText(_tank.KillCount);
    }
    private void SetName(string newName)
    {
        _nameText.text = newName;
    }
    private void UpdateKillCountText(int val)
    {
        if(val > 0)
        {
            _killCountText.text = val.ToString();
        }
        else
        {
            _killCountText.text = "";
        }
    }
    private void SetAlive()
    {
        _aliveBg.enabled = true;
        _deadBg.enabled = false;
    }
    private void SetDead()
    {
        _aliveBg.enabled = false;
        _deadBg.enabled = true;
    }
}
