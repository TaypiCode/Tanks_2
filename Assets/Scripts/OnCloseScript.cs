using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCloseScript : MonoBehaviour
{
    [SerializeField] private bool _needSaveOnThisPage;
    private SaveGame _save;
    private void Start()
    {
        _save = FindObjectOfType<SaveGame>();
    }
    public void OnClose()
    {
        if (_needSaveOnThisPage)
        {
            _save.SaveProgress();
        }
    }
}
