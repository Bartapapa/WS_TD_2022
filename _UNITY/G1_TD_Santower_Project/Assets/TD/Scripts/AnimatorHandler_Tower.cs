using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler_Tower : AnimatorHandler
{
    [Header("Animations")]
    [SerializeField]
    private int _baseAnimationIndex = 0;
    [SerializeField]
    private int _cannonAnimationIndex = 0;

    public void Initialize()
    {
        _anim.SetInteger("BInt", _baseAnimationIndex);
        _anim.SetInteger("CInt", _cannonAnimationIndex);
    }
}
