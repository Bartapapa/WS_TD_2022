using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaAbilitiesController : MonoBehaviour
{
    [SerializeField]
    private List<AbilitySlot> _abilitySlots = new List<AbilitySlot>();

    private void Awake()
    {
        foreach (AbilitySlot abilitySlot in _abilitySlots)
        {
            abilitySlot.gameObject.SetActive(false);
        }
    }

    public void RevealAbility(int value)
    {
        Debug.Log("Revealed " + _abilitySlots[value].Ability.Name + "!");
        _abilitySlots[value].gameObject.SetActive(true);
    }
}
