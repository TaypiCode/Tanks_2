using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageUIEffect : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private Image _image;
    [SerializeField] private float _showTime;
    [SerializeField] private float _maxAlpha;
    private float _time = 0;
    private void Start()
    {
        StartCoroutine(UpdateTimer());
    }
    private void Update()
    {
        _color.a = Mathf.Clamp(_time, 0, _maxAlpha);
        _image.color = _color;
    }
    public void GetDamage()
    {
        SetTimer(_showTime);
    }
    private IEnumerator UpdateTimer()
    {
        while (true)
        {
            if (_time >= 0)
            {
                _time -= Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private void SetTimer(float time)
    {
        _time = time;
    }
}
