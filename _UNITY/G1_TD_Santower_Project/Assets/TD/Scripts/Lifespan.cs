using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifespan : MonoBehaviour
{
    [SerializeField]
    private Timer _lifeTimer;

    private void Awake()
    {
        _lifeTimer.Start();
    }

    private void Update()
    {
        _lifeTimer.Update();

        if (_lifeTimer.Progress >= 1)
        {
            Destroy(this.gameObject);
        }
    }
}
