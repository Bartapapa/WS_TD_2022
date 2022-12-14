using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ability_Barricade : Ability
{
    [SerializeField]
    private AbilitySlot _abilitySlot;

    [SerializeField]
    private Barricade _barricade1;
    [SerializeField]
    private Barricade _barricade2;
    [SerializeField]
    private Barricade _instantiatedBarricade;

    [SerializeField]
    private bool _upgraded = false;

    private void Awake()
    {
        _abilitySlot = GetComponent<AbilitySlot>();
    }

    public override void RequestAbility(AbilityDescription abilityDescription)
    {
        if (_requested) return;
        if (!_isReady) return;

        _requested = true;


        Barricade newBarricade;
        if (_upgraded)
        {
            newBarricade = Instantiate(_barricade2);
        }
        else
        {
            newBarricade = Instantiate(_barricade1);
        }
        _instantiatedBarricade = newBarricade;

        newBarricade.SetCanBlock(true);

        LevelReferences.Instance.PlayerPickerController.CreateGhost(newBarricade);
        LevelReferences.Instance.PlayerPickerController.SetTargetingReticle(abilityDescription.TargetingReticle, 1, 1, false);

        LevelReferences.Instance.PlayerPickerController.ChangeState(PlayerPickerState.Targeting);
    }

    public override void ActivateAbility(Vector3 position, Vector3 direction)
    {
        if (!_requested) return;
        _isReady = false;
        _requested = false;

        _abilitySlot.StartCooldownTimer();
        LevelReferences.Instance.PlayerPickerController.ChangeState(PlayerPickerState.InGame);
    }

    public override void ReadyAbility()
    {
        _isReady = true;
    }

    public override void UnrequestAbility()
    {
        _requested = false;
        LevelReferences.Instance.PlayerPickerController.ChangeState(PlayerPickerState.InGame);
    }

    public void UpgradeBarricades()
    {
        _upgraded = true;
    }
}
