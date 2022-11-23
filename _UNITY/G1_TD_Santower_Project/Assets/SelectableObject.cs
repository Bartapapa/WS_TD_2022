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

    public bool IsSelected => _isSelected;


    private void Awake()
    {
        //_selectionCircle.SetActive(false);
        //_selectionCircleHover.SetActive(false);
    }

    public void Select()
    {
        //_selectionCircle.SetActive(true);
        //_selectionCircleHover.SetActive(false);
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
}
