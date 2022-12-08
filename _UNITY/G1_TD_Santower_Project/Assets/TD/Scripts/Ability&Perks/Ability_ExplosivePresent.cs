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
    private float _presentExplosionRadius = 7f;
    [SerializeField]
    private int _presentDamage = 50;

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
        newPresent.SetExplosionDamage(_presentDamage);
        newPresent.SetExplosionRadius(_presentExplosionRadius);

        LevelReferences.Instance.PlayerPickerController.CreateGhost(newPresent);
        LevelReferences.Instance.PlayerPickerController.SetTargetingReticle(abilityDescription.TargetingReticle, _presentExplosionRadius, _presentExplosionRadius, false);

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

    public void UpgradePresent()
    {
        //I could really do this by creating a sort of dictionary which takes in the AbilityDescription's level and outputs a thing regarding what bonuses that level grants.
        //But it's friday, and I need to be quick to ge this done before the end of the day so we can have a working build.
        _presentExplosionRadius = 10f;
        _presentDamage = 75;
    }
}
