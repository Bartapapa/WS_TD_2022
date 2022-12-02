using GSGD1;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_ExplosivePresent : Ability
{
    [SerializeField]
    private ExplosivePresent _explosivePresent;
    private ExplosivePresent _instantiatedPresent;

    [SerializeField]
    private AbilitySlot _abilitySlot;

    [SerializeField]
    private float _width = 1f;
    [SerializeField]
    private float _length = 1f;

    private void Awake()
    {
        _abilitySlot = GetComponent<AbilitySlot>();
    }

    public override void RequestAbility(AbilityDescription abilityDescription)
    {
        if (_requested) return;
        if (!_isReady) return;

        _requested = true;

        ExplosivePresent newPresent = Instantiate(_explosivePresent);
        _instantiatedPresent = newPresent;

        LevelReferences.Instance.PlayerPickerController.CreateGhost(newPresent);
        LevelReferences.Instance.PlayerPickerController.SetTargetingReticle(abilityDescription.TargetingReticle, _width, _length, false);

        LevelReferences.Instance.PlayerPickerController.ChangeState(PlayerPickerState.Targeting);
    }

    public override void ActivateAbility(Vector3 position, Vector3 direction)
    {
        if (!_requested) return;
        _isReady = false;
        _requested = false;

        _instantiatedPresent.transform.position = LevelReferences.Instance.NorthPole.GetAimPosition();
        _instantiatedPresent.ThrowPresent(position, direction);

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
}
