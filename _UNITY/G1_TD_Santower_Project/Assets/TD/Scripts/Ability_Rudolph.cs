using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ability_Rudolph : Ability
{
    public UnityEvent RenewRequestAbility;

    [SerializeField]
    private RudolphSleigh _sleigh;
    [SerializeField]
    private RudolphSleigh _instantiatedSleigh;

    [SerializeField]
    private AbilitySlot _abilitySlot;

    [SerializeField]
    private float _width = 1f;
    [SerializeField]
    private float _length = 1f;

    [SerializeField]
    private int _level = 1;
    [SerializeField]
    private int _maxnumberOfSleighs = 1;
    [SerializeField]
    private int _sleighsShot = 0;

    private void Awake()
    {
        _abilitySlot = GetComponent<AbilitySlot>();
    }

    public override void RequestAbility(AbilityDescription abilityDescription)
    {
        if (_requested) return;
        if (!_isReady) return;

        _requested = true;

        LevelReferences.Instance.PlayerPickerController.SetTargetingReticle(abilityDescription.TargetingReticle, _width, _length, false);

        LevelReferences.Instance.PlayerPickerController.ChangeState(PlayerPickerState.Targeting);
    }

    public override void ActivateAbility(Vector3 position, Vector3 secondPosition)
    {
        if (!_requested) return;
        _requested = false;

        RudolphSleigh newSleigh = Instantiate(_sleigh);
        _instantiatedSleigh = newSleigh;

        _sleighsShot += 1;

        if (_level == 1)
        {
            newSleigh.transform.position = position;
            float randomRotation = Random.Range(0, 360);
            newSleigh.transform.Rotate(0, randomRotation, 0);
            newSleigh.transform.position = new Vector3(newSleigh.transform.position.x, 5f, newSleigh.transform.position.z);
            newSleigh.transform.position = newSleigh.transform.position + newSleigh.transform.forward * -10f;
            newSleigh.SetTargetPosition(position);
        }
        else if (_level == 2)
        {
            newSleigh.transform.position = position;
            newSleigh.transform.LookAt(secondPosition);
            newSleigh.transform.position = new Vector3(newSleigh.transform.position.x, 5f, newSleigh.transform.position.z);
            newSleigh.transform.position = newSleigh.transform.position + newSleigh.transform.forward * -10f;
            newSleigh.StartBombing();
        }

        if(_sleighsShot < _maxnumberOfSleighs)
        {
            RenewRequestAbility.Invoke();
        }
        else
        {
            _isReady = false;
            _sleighsShot = 0;
            _abilitySlot.StartCooldownTimer();
            LevelReferences.Instance.PlayerPickerController.ChangeState(PlayerPickerState.InGame);
        }

        //_abilitySlot.StartCooldownTimer();
        //LevelReferences.Instance.PlayerPickerController.ChangeState(PlayerPickerState.InGame);
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
