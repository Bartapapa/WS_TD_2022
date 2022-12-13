using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCannon : MonoBehaviour
{
    [SerializeField]
    private TowerBase _towerBase;

    [SerializeField]
    private Transform _anchor;

    [SerializeField]
    private Vector3 _forceOverride;

    private Quaternion _lastLookRotation = Quaternion.identity;

    public void CannonLookAt(Vector3 position, float rotationSpeed, bool onlyCanonRotates = false)
    {
        ////TODO force anchor to follow WeaponController.forward.

        //Vector3 lookPos = position - transform.position;
        //Quaternion lookRotation = Quaternion.LookRotation(lookPos);
        //_anchor.rotation = lookRotation;

        //Vector3 positionIgnoreXZ = new Vector3(basePosition.x, targetPosition.y, basePosition.z);

        Vector3 direction = (position - transform.position).normalized;

        _lastLookRotation = Quaternion.LookRotation(position + _forceOverride, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lastLookRotation, rotationSpeed * Time.deltaTime);
        if (onlyCanonRotates)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, _towerBase.transform.eulerAngles.y, 0);
        }

        //transform.rotation = Quaternion.Euler(transform.rotation.x, 0, 0);

        _anchor.rotation = Quaternion.LookRotation(position, Vector3.up);

        if (onlyCanonRotates)
        {
            _anchor.eulerAngles = new Vector3(_anchor.eulerAngles.x, transform.eulerAngles.y, 0);
        }
        else
        {
            _anchor.eulerAngles = new Vector3(_anchor.eulerAngles.x, _towerBase.transform.eulerAngles.y, 0);
        }

        //_anchor.rotation = Quaternion.Euler(transform.rotation.x, 0, 0);
    }
}
