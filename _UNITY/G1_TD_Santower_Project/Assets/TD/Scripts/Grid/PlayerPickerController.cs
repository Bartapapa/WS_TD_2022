namespace GSGD1
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class PlayerPickerController : MonoBehaviour
	{
		[SerializeField]
		private GridBehaviour _grid = null;

		[SerializeField]
		private GridPicker _gridPicker = null;

        [Header("Selectables")]
        [SerializeField]
        private LayerMask _selectableLayer;
        [SerializeField]
        private LayerMask _UILayer;
        [SerializeField]
        private SelectableObject _currentSelectable;
        [SerializeField]
        private SelectableObject _hoveringOver;

        [System.NonSerialized]
		private IPickerGhost _ghost = null;

		[System.NonSerialized]
		private bool _isActive = false;

		public void Activate(bool isActive)
		{
			_isActive = isActive;
			_gridPicker.Activate(isActive, true);
		}

		public void ActivateWithGhost(IPickerGhost ghost)
		{
			_ghost = ghost;
			Activate(true);
		}

		public void DestroyGhost()
		{
			if (_ghost != null)
			{
				Destroy(_ghost.GetTransform().gameObject);
				_ghost = null;
			}
		}

		public bool TrySetGhostAsCellChild()
		{
			if (_gridPicker.TryGetCell(out Cell cell) == true)
			{
				if (cell.HasChild == false)
				{
					if (cell.SetChild(_ghost as ICellChild) == true)
					{
						_ghost = null;
						return true;
					}
				}
			}
			return false;
		}

		private void Update()
		{
			if (_isActive == true)
			{
				if (_gridPicker.TryGetCell(out Cell cell) == true)
				{
					_ghost.GetTransform().position = _grid.GetCellCenter(_gridPicker.CellPosition);
				}
				else if (_ghost != null)
				{
					_ghost.GetTransform().position = _gridPicker.HitPosition;
				}
			}
			else
			{
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, float.MaxValue, _UILayer))
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        if (_currentSelectable != null)
                        {
                            _currentSelectable.StopSelecting();
                            _currentSelectable = null;
                        }
                    }
                    return;
                }

                if (Physics.Raycast(ray, out hit, float.MaxValue, _selectableLayer))
                {
                    SelectableObject selectable = hit.collider.gameObject.GetComponent<SelectableObject>();
                    if (selectable != null)
                    {
                        selectable.HoverOver();
                        _hoveringOver = selectable;
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (_hoveringOver != null)
                        {
                            if (_currentSelectable != null)
                            {
                                _currentSelectable.StopSelecting();
                            }

                            _hoveringOver.Select();
                            _currentSelectable = _hoveringOver;
                        }
                    }
                }
                else
                {
                    if (_hoveringOver != null)
                    {
                        _hoveringOver.StopHovering();
                    }
                    _hoveringOver = null;

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (_currentSelectable != null)
                        {
                            _currentSelectable.StopSelecting();
                            _currentSelectable = null;
                        }
                    }
                }
                if (Input.GetMouseButtonDown(1))
                {
                    if (_currentSelectable != null)
                    {
                        _currentSelectable.StopSelecting();
                        _currentSelectable = null;
                    }
                }
            }

        }

		[ContextMenu("Activate")]
		private void DoActivate() => Activate(true);

		[ContextMenu("Deactivate")]
		private void DoDeactivate() => Activate(false);

	}
}