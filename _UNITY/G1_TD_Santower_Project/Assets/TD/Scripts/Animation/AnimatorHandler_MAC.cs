using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler_MAC : AnimatorHandler_Entity
{
    [SerializeField]
    private Transform _MAC;

    [SerializeField]
    private Transform _horse;

    void Start()
    {
        _MAC.transform.parent = _horse;
    }
}
