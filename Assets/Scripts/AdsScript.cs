using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices; //need to call js

public class AdsScript : MonoBehaviour
{
    [SerializeField] private Battle _battle;
    private bool _canShowNoRewardAds = true;
    private float _noRewardAdsDelay = 185; //min +180 (by yandex)
    private static float _noRewardAdsTimer;
    private static bool _timerStarted;


    public enum Placements 
    {
        BattleRewarded
    }


    [DllImport("__Internal")]
    private static extern void ShowRewardAd(string id); //call js from plugin UnityScriptToJS.jslib
    [DllImport("__Internal")]
    private static extern void ShowInterstitialAd(); //call js from plugin UnityScriptToJS.jslib
    private void Start()
    {
        if (_timerStarted == false)
        {
            _timerStarted = true;
            _noRewardAdsTimer = _noRewardAdsDelay;
        }
    }
    private void Update()
    {
        _noRewardAdsTimer -= Time.deltaTime;
    }
    public void ShowAd(Placements placement)
    {
        ShowRewardAd(placement.ToString());

    }
    public void ShowNonRewardAd()
    {
        if (_canShowNoRewardAds && _noRewardAdsTimer < 0)
        {
            ShowInterstitialAd();
        }
    }
    public void OnNonRewardAdsShowed()
    {
        _noRewardAdsTimer = _noRewardAdsDelay;
    }
    public void OnAdsReward(string placement)
    {
        if (placement == Placements.BattleRewarded.ToString())
        {
            if(_battle != null)
            {
                _battle.GetAdsReward();
            }
        }
    }
    public void ShowBattleRewardAds()
    {
        ShowAd(Placements.BattleRewarded);
    }
    public void OnAdsError()
    {
        SetTimeScale(1);
    }
    public void OnAdsOpen()
    {

        SetTimeScale(0);
    }
    public void OnAdsClose()
    {
        SetTimeScale(1);
    }
    private void SetTimeScale(float val)
    {
        Time.timeScale = val;
        AudioListener.volume = val;
    }
    public void SetCanShowNoRewardAds(bool val)
    {
        _canShowNoRewardAds = val;
    }
}