using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTanksScript : MonoBehaviour
{
    [SerializeField] private List<TankScriptableObject> _allTanks;

    public List<TankScriptableObject> AllTanks { get => _allTanks; }
}
