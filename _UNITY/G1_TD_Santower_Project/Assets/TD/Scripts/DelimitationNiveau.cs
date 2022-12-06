using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelimitationNiveau : MonoBehaviour
{
    [SerializeField]
    private float myPlayingFieldRadius1 = 1f;

    [SerializeField]
    private float myPlayingFieldRadius2 = 1f;

    [SerializeField]
    private float myPlayingFieldRadius3 = 1f;




    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, myPlayingFieldRadius1);
        Gizmos.DrawWireSphere(transform.position, myPlayingFieldRadius2);
        Gizmos.DrawWireSphere(transform.position, myPlayingFieldRadius3);
    }
}
