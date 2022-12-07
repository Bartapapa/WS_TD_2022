using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _baseGO;

    public void ForceDestroy()
    {
        Destroy(_baseGO);
    }
}
