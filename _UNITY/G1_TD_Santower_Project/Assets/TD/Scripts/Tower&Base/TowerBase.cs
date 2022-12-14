using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerBase : MonoBehaviour
{
    private Vector3 _originalRotation = new Vector3(-90, 0, 0);
    private Quaternion _lastLookRotation = Quaternion.identity;

    private void Start()
    {
        _originalRotation = transform.localRotation.eulerAngles;
    }

    public void BaseLookAt(Vector3 position, float rotationSpeed, bool onlyCanonRotates = false)
    {
        if (onlyCanonRotates) return;

        Vector3 direction = (position - transform.position).normalized;

        _lastLookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lastLookRotation, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(_originalRotation.x, transform.eulerAngles.y, _originalRotation.z);
    }
}
