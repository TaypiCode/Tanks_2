using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillChatMessageUI : MonoBehaviour
{
    [SerializeField] private Image _bg;
    [SerializeField] private TextMeshProUGUI _killerText;
    [SerializeField] private TextMeshProUGUI _killedText;
    [SerializeField] private float _showTime;
    [SerializeField] private Color _playerTeamColor;
    [SerializeField] private Color _enemyTeamColor;
    private IEnumerator LiveTimer()
    {
        yield return new WaitForSeconds(_showTime);
        Destroy(this.gameObject);
    }
    public void ShowMessage(string killerName, string killedName, bool isPlayerTeam)
    {
        if (isPlayerTeam)
        {
            _bg.color = _playerTeamColor;
        }
        else
        {
            _bg.color = _enemyTeamColor;
        }
        _killerText.text = killerName;
        _killedText.text = killedName;
        StartCoroutine(LiveTimer());
    }
}
