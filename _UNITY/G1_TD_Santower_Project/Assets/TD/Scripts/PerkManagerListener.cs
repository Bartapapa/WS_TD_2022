using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PerkManagerListener : MonoBehaviour
{
    [SerializeField] private PerkDescription _listeningFor;

    public UnityEvent OnPerkAcquired_UE;


    private void OnEnable()
    {
        PlayerPerkManager.Instance.PerkAcquired -= OnPerkAcquired;
        PlayerPerkManager.Instance.PerkAcquired += OnPerkAcquired;
    }

    private void OnDisable()
    {
        PlayerPerkManager.Instance.PerkAcquired -= OnPerkAcquired;
    }

    private void OnPerkAcquired(int newPerkIndex, string perkName)
    {
        if (perkName == _listeningFor.Name)
        {
            OnPerkAcquired_UE.Invoke();
        }
    }
}
