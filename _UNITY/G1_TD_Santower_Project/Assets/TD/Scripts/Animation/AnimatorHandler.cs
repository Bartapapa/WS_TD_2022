using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    [SerializeField]
    protected Animator _anim;

    public Animator Animator => _anim;

    protected virtual void Awake()
    {
        if (_anim == null)
        {
            Debug.Log(name + " doesn't have an animator. Please advise.");
        }
    }
}
