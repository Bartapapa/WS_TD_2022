using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ability_RPG : Ability
{
    public UnityEvent RenewRequestAbility;

    [SerializeField]
    private ProjectileExplosive _missile;
    [SerializeField]
    private ProjectileExplosive _instantiatedMissile;

    [SerializeField]
    private AbilitySlot _abilitySlot;

    [SerializeField]
    private float _width = 1f;
    [SerializeField]
    private float _length = 1f;

    [SerializeField]
    private int _maxnumberOfMissiles = 1;
    [SerializeField]
    private int _missilesShot = 0;

    private void Awake()
    {
        _abilitySlot = GetComponent<AbilitySlot>();
    }

    public override void RequestAbility(AbilityDescription abilityDescription)
    {
        if (_requested) return;
        if (!_isReady) return;

        _requested = true;

        LevelReferences.Instance.PlayerPickerController.SetTargetingReticle(abilityDescription.TargetingReticle, _width, _length, true);

        LevelReferences.Instance.PlayerPickerController.ChangeState(PlayerPickerState.Targeting);
    }

    public override void ActivateAbility(Vector3 position, Vector3 direction)
    {
        if (!_requested) return;
        _requested = false;

        ProjectileExplosive newMissile = Instantiate(_missile);
        _instantiatedMissile = newMissile;
        newMissile.transform.LookAt(position);

        _missilesShot += 1;
        if (_missilesShot < _maxnumberOfMissiles)
        {
            RenewRequestAbility.Invoke();
        }
        else
        {
            _isReady = false;
            _missilesShot = 0;
            _abilitySlot.StartCooldownTimer();
            LevelReferences.Instance.PlayerPickerController.ChangeState(PlayerPickerState.InGame);
        }

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
