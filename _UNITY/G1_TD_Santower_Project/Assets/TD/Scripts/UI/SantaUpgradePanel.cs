using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaUpgradePanel : MonoBehaviour
{
    [SerializeField]
    private List<PerkSlot> _perkSlots = new List<PerkSlot>();

    public List<PerkSlot> PerkSlots => _perkSlots;

    public void InitializePanel()
    {
        if (_perkSlots.Count < PlayerPerkManager.Instance.PerkList.Count)
        {
            Debug.Log("Not as many perk slots as there are perks. Please advise.");
        }
        else
        {
            for (int i = 0; i < PlayerPerkManager.Instance.PerkList.Count; i++)
            {
                _perkSlots[i].InitializeSlot(PlayerPerkManager.Instance.PerkList[i]);
                _perkSlots[i].SetIsAcquired(false);
                _perkSlots[i].SetIsAvailable(false);
                _perkSlots[i].UpdateSlot();
            }
        }
    }
}
