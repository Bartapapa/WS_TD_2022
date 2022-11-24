using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetingReticleType
{
    None,
    Point,
    Area,
    Road,
}

[CreateAssetMenu(menuName = "GameSup/Targeting Reticle", fileName = "TargetingReticle")]
public class TargetingReticle : ScriptableObject
{
    [SerializeField] private GameObject _prefab;

    [SerializeField] private TargetingReticleType _targetingReticle = TargetingReticleType.Point;

    public TargetingReticleType TargetingReticleType => _targetingReticle;

    public GameObject Prefab => _prefab;
}
