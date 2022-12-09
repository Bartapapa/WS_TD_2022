using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public delegate void SelectEvent(SelectableObject selectableObject, bool isSelected);
    public event SelectEvent ObjectSelected = null;

    //[SerializeField]
    //private GameObject _selectionCircle;

    //[SerializeField]
    //private GameObject _selectionCircleHover;

    [SerializeField]
    private bool _isSelected = false;

    [SerializeField]
    private bool _canBeSelected = false;

    public bool IsSelected => _isSelected;
    public bool CanBeSelected => _canBeSelected;


    private void Awake()
    {
        //_selectionCircle.SetActive(false);
        //_selectionCircleHover.SetActive(false);
    }

    public void Select()
    {
        //_selectionCircle.SetActive(true);
        //_selectionCircleHover.SetActive(false);
        if (!_canBeSelected) return;

        _isSelected = true;

        if (ObjectSelected != null)
        {
            ObjectSelected.Invoke(this, true);
        }
    }

    public void HoverOver()
    {
        if (_isSelected) return;
        //_selectionCircle.SetActive(false);
        //_selectionCircleHover.SetActive(true);
    }

    public void StopSelecting()
    {
        //_selectionCircle.SetActive(false);
        //_selectionCircleHover.SetActive(false);
        _isSelected = false;

        if (ObjectSelected != null)
        {
            ObjectSelected.Invoke(this, false);
        }
    }

    public void StopHovering()
    {
        if (_isSelected) return;
        //_selectionCircleHover.SetActive(false);
    }

    public void SetCanBeSelected(bool value)
    {
        StartCoroutine(SelectionGracePeriod(.1f, value));
    }

    private IEnumerator SelectionGracePeriod(float duration, bool value)
    {
        float timer = 0f;
        float currentTime = duration;

        while (timer < currentTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        _canBeSelected = value;

        yield return null;
    }
}
