using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField]
    protected bool _requested = false;

    [SerializeField]
    protected bool _isReady = true;

    private void OnEnable()
    {
        LevelReferences.Instance.PlayerPickerController.PlayerPickerTargetingConfirmed -= ActivateAbility;
        LevelReferences.Instance.PlayerPickerController.PlayerPickerTargetingConfirmed += ActivateAbility;

        LevelReferences.Instance.PlayerPickerController.PlayerPickerRequestDenied -= UnrequestAbility;
        LevelReferences.Instance.PlayerPickerController.PlayerPickerRequestDenied += UnrequestAbility;
    }

    private void OnDisable()
    {
        LevelReferences.Instance.PlayerPickerController.PlayerPickerTargetingConfirmed -= ActivateAbility;

        LevelReferences.Instance.PlayerPickerController.PlayerPickerRequestDenied -= UnrequestAbility;
    }

    public virtual void RequestAbility(AbilityDescription abilityDescription)
    {
        Debug.Log(_requested);
        Debug.Log(_isReady);

        if (_requested) return;
        if (!_isReady) return;

        _requested = true;
    }
    public virtual void UnrequestAbility()
    {
        _requested = false;
    }

    public virtual void ReadyAbility()
    {
        _isReady = true;
    }

    public virtual void ActivateAbility(Vector3 position, Vector3 direction)
    {
        if (!_requested) return;
        _isReady = false;
        _requested = false;

        //Actually do the ability.
        //Take off the ghost from PPController, as well as its TargetingReticle. Set the targeted position as requestedPosition.
        //place present at Santa's tower, and make him throw the present. Once the present hits the ground, it waits for a bit before setting _canExplode to true.
    }
}
