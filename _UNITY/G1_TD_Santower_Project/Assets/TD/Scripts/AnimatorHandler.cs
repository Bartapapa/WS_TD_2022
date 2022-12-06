using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    [SerializeField]
    private Animator _anim;

    [Header("Animations")]
    [SerializeField]
    private int _maxWalkAnims = 0;
    [SerializeField]
    private int _maxDeathAnims = 0;

    private int integer;

    public Animator Animator => _anim;

    private void Awake()
    {
        if (_anim == null)
        {
            Debug.Log(name + " doesn't have an animator. Please advise.");
        }
    }

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
