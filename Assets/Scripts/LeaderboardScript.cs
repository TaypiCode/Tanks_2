using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices; //need to call js
public class LeaderboardScript : MonoBehaviour
{
    [SerializeField] private bool _needUse = true;
    private static bool _needUseStatic;
    public enum Names
    {
        Rating
    };
    [DllImport("__Internal")]
    private static extern void SetLeaderboardData(string leaderboadName, float leaderboardValue); //call js from plugin UnityScriptToJS.jslib
    private void Start()
    {
        _needUseStatic = _needUse;
    }
    public static void SetLeaderboardValue(Names leaderboardName, float val)
    {
        if(_needUseStatic) SetLeaderboardData(leaderboardName.ToString(), val);
    }
}
