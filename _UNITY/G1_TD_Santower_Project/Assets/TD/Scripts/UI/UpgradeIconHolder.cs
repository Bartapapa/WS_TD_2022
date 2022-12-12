using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeIconHolder : MonoBehaviour
{
    [SerializeField]
    private Transform _iconParent;

    public Transform IconParent => _iconParent;
}
