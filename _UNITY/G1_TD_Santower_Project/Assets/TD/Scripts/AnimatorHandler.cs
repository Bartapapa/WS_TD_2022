using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    [SerializeField]
    private Animator _anim;

    private int integer;

    public Animator Animator => _anim;

    private void Awake()
    {
        if (_anim == null)
        {
            Debug.Log(name + " doesn't have an animator. Please advise.");
        }
    }

    public virtual void SetInteger(bool isRandom, int valueMaxExclusive)
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
