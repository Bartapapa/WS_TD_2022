using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour
{
    private Slider _slider;

    private void Awake()
    {
        _slider = transform.GetComponent<Slider>();
    }

    private void Update()
    {
        _slider.value = Loader.GetLoadingProgress();
    }
}
