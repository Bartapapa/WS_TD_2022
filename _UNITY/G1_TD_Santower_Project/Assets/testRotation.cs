using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRotation : MonoBehaviour
{
    [SerializeField]
    private float _rotSpeed;

    private void Update()
    {
        transform.Rotate(0, _rotSpeed * Time.deltaTime, 0);
    }
}
