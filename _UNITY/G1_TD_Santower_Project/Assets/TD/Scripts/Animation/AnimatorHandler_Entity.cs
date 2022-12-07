using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler_Entity : AnimatorHandler
{
    [Header("Entity Animations")]
    [SerializeField]
    private int _maxWalkAnims = 0;
    [SerializeField]
    private int _maxDeathAnims = 0;

    private int integer;

    public virtual void SetWalkAnimation()
    {
        SetInteger(true, _maxWalkAnims);
    }

    public virtual void SetDeathAnimation()
    {
        SetInteger(true, _maxDeathAnims);
    }

    private void SetInteger(bool isRandom, int valueMaxExclusive)
    {
        if (isRandom)
        {
            integer = Random.Range(0, valueMaxExclusive);
        }
        else
        {
            integer = valueMaxExclusive;
        }

        _anim.SetInteger("Int", integer);
    }
}
