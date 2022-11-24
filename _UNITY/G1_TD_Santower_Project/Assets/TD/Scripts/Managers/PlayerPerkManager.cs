using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerkManager : Singleton<PlayerPerkManager>
{
    public delegate void PerkManagerEvent(int newPerkIndex, string perkName);
    public event PerkManagerEvent PerkAcquired = null;

    [SerializeField] private int _currentPerk = 0;

    [SerializeField] private List<PerkDescription> _perkOrder = new List<PerkDescription>();

    public int CurrentPerk => _currentPerk;

    public void AcquirePerk(int perkIndex)
    {
        if (ResourceManager.Instance.CanBuy(ResourceManager.ResourceType.Milk, _perkOrder[perkIndex].MilkCost))
        {
            if (perkIndex > _perkOrder.Count - 1)
            {
                Debug.Log("There aren't that many perks!");
                return;
            }

            if (_currentPerk < perkIndex)
            {
                _currentPerk = perkIndex;
                ResourceManager.Instance.AcquireResource(ResourceManager.ResourceType.Milk, -_perkOrder[perkIndex].MilkCost);

                PerkAcquired?.Invoke(_currentPerk, _perkOrder[_currentPerk].name);
                Debug.Log(_perkOrder[_currentPerk].name + " acquired.");
            }
            else
            {
                Debug.Log("You've already got that perk!");
            }
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    AcquirePerk(_currentPerk + 1);
        //}
    }
}
