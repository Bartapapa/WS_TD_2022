using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSup/Perk Description", fileName = "PerkDescription")]
public class PerkDescription : ScriptableObject
{
    [SerializeField]
    private string _name = "PerkDefaultName";

    [SerializeField]
    private Sprite _icon = null;

    [SerializeField]
    private int _milkCost = 0;

    public int MilkCost => _milkCost;
}
