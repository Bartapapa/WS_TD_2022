using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSup/Ability Description", fileName = "AbilityDescription")]
public class AbilityDescription : ScriptableObject
{
    [SerializeField]
    private string _name = "AbilityDefaultName";

    [SerializeField]
    private TargetingReticle _targetingReticle;

    [SerializeField]
    private Sprite _icon = null;

    [SerializeField]
    private float _coolDown = 1f;

    public string Name => _name;

    public TargetingReticle TargetingReticle => _targetingReticle;

    public Sprite Icon => _icon;

    public float CoolDown => _coolDown;

    public void SetCoolDown(float value)
    {
        _coolDown = value;
    }

    public void Initialize(string name, TargetingReticle targetingReticle, Sprite icon, float coolDown)
    {
        _name = name;
        _targetingReticle = targetingReticle;
        _icon = icon;
        _coolDown = coolDown;
    }
}
