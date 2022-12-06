using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTourelle : MonoBehaviour
{
    [SerializeField]
    private float TowerLevel1= 1f;

    [SerializeField]
    private float TowerLevel2Sol = 1f;

    [SerializeField]
    private float TowerLevel2Zone = 1f;

    [SerializeField]
    private float TowerLevel3ZoneB = 1f;

    [SerializeField]
    private float TowerLevel3ZoneA = 1f;

    [SerializeField]
    private float TowerLevel3SolA = 1f;

    [SerializeField]
    private float TowerLevel3SolB = 1f;

    [SerializeField]
    private bool seeT1 = false;

    [SerializeField]
    private bool seeT2 = false;

    [SerializeField]
    private bool seeT3Sol = false;

    [SerializeField]
    private bool seeT3Zone = false;


    private void OnDrawGizmos()
    {
        if (seeT1 == true)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, TowerLevel1);
        }
       
        if (seeT2 == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, TowerLevel2Sol);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, TowerLevel2Zone);
        }
        
        if (seeT3Sol == true)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, TowerLevel3SolA);
            Gizmos.DrawWireSphere(transform.position, TowerLevel3SolB);
        }

        if (seeT3Zone == true)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, TowerLevel3ZoneB);
            Gizmos.DrawWireSphere(transform.position, TowerLevel3ZoneA);
        }
       

        
    }
}
