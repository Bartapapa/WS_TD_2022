namespace GSGD1
{
	using System.Collections;
	using System.Collections.Generic;
    using UnityEngine;

    public enum PlayerPickerState
    {
        InGame,
        PlacingObject,
        InUI,
        Targeting,
        None,
    }

    public class PlayerPickerController : MonoBehaviour
	{
        public delegate void PlayerPickerStateEvent(PlayerPickerState currentState, PlayerPickerState newState);
        public event PlayerPickerStateEvent StateChanged = null;

        public delegate void PlayerPickerTargetingEvent(Vector3 position, Vector3 direction);
        public event PlayerPickerTargetingEvent PlayerPickerTargetingConfirmed = null;
        public delegate void PlayerPickerTargetingChoiceEvent();
        public event PlayerPickerTargetingChoiceEvent PlayerPickerRequestDenied = null;

        [SerializeField]
		private GridBehaviour _grid = null;

		[SerializeField]
		private GridPicker _gridPicker = null;

        [Header("State")]
        [SerializeField]
        private PlayerPickerState _state = PlayerPickerState.InGame;

        [Header("Selectables")]
        [SerializeField]
        private LayerMask _selectableLayer;
        [SerializeField]
        private LayerMask _UILayer;
        [SerializeField]
        private SelectableObject _currentSelectable;
        [SerializeField]
        private SelectableObject _hoveringOver;

        [Header("Targeting")]
        [SerializeField]
        private TargetingReticle _targetingReticleDescription;
        [SerializeField]
        private GameObject _targetingReticle = null;
        [SerializeField]
        private LayerMask _groundLayer;
        [SerializeField]
        private Transform _targetingParent;
        private bool _reticleLocked = false;

        [System.NonSerialized]
		private IPickerGhost _ghost = null;

		[System.NonSerialized]
		private bool _isActive = false;

		public void Activate(bool isActive)
		{
			_isActive = isActive;
			_gridPicker.Activate(isActive, true);

            if (isActive == true)
            {
                ChangeState(PlayerPickerState.PlacingObject);
            }
            else
            {
                ChangeState(PlayerPickerState.InGame);
            }
        }

		public void ActivateWithGhost(IPickerGhost ghost)
		{
			_ghost = ghost;
			Activate(true);
		}

        public void CreateGhost(IPickerGhost ghost)
        {
            _ghost = ghost;
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

		public bool TrySetGhostAsPlateChild()
		{
			if (_gridPicker.TryGetPlate(out Plate plate) == true)
			{
				if (plate.HasChild == false || plate.HasChild == true)
				{
					if (plate.SetChild(_ghost as IPlateChild) == true)
					{
						_ghost = null;
						return true;
					}
				}
			}
			return false;
		}

        public void ChangeState(PlayerPickerState newState)
        {
            if (newState == _state) return;
            else
            {
                StateChanged?.Invoke(_state, newState);
                _state = newState;
            }
        }

        private void Update()
		{
			if (_state == PlayerPickerState.PlacingObject)
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
			else if (_state == PlayerPickerState.InGame)
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

                if (Physics.Raycast(ray, out hit, float.MaxValue, _selectableLayer, QueryTriggerInteraction.Ignore))
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
            else if (_state == PlayerPickerState.InUI)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //How the picker behaves in UI, such as when clicking on a tower or on Santa.

                if (Input.GetMouseButtonDown(1))
                {
                    ChangeState(PlayerPickerState.InGame);
                }           
            }
            else if (_state == PlayerPickerState.Targeting)
            {
                if (_targetingReticle != null)
                {
                    if (!_targetingReticle.activeInHierarchy)
                    {
                        _targetingReticle.SetActive(true);
                    }
                }


                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                switch (_targetingReticleDescription.TargetingReticleType)
                {
                    case TargetingReticleType.None:
                        {
                            DestroyGhost();
                            PlayerPickerRequestDenied?.Invoke();
                            ChangeState(PlayerPickerState.InGame);
                        }
                        
                        break;
                    case TargetingReticleType.Point:
                        {
                            if (Physics.Raycast(ray, out hit, float.MaxValue, _groundLayer))
                            {
                                if (_ghost != null)
                                {
                                    _ghost.GetTransform().position = hit.point;
                                }
                                if (_targetingReticle != null)
                                {
                                    _targetingReticle.transform.position = hit.point;
                                }

                                if (Input.GetMouseButtonDown(0))
                                {
                                    _ghost = null;
                                    DestroyPreviousTargetingReticle();
                                    PlayerPickerTargetingConfirmed?.Invoke(hit.point, Vector3.zero);
                                    ChangeState(PlayerPickerState.InGame);
                                }
                            }

                            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                            {
                                if (_targetingReticle.activeInHierarchy)
                                {
                                    _targetingReticle.SetActive(false);
                                }
                                DestroyGhost();
                                PlayerPickerRequestDenied?.Invoke();
                                ChangeState(PlayerPickerState.InGame);
                            }
                        }
                        break;
                    case TargetingReticleType.Area:
                        {
                            if (Physics.Raycast(ray, out hit, float.MaxValue, _groundLayer))
                            {
                                if (_ghost != null)
                                {
                                    _ghost.GetTransform().position = hit.point;
                                }
                                if (_targetingReticle != null)
                                {
                                    _targetingReticle.transform.position = hit.point;
                                }

                                if (Input.GetMouseButtonDown(0))
                                {
                                    _ghost = null;
                                    DestroyPreviousTargetingReticle();
                                    PlayerPickerTargetingConfirmed?.Invoke(hit.point, Vector3.zero);
                                    ChangeState(PlayerPickerState.InGame);
                                }
                            }

                            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                            {
                                if (_targetingReticle.activeInHierarchy)
                                {
                                    _targetingReticle.SetActive(false);
                                }
                                DestroyGhost();
                                PlayerPickerRequestDenied?.Invoke();
                                ChangeState(PlayerPickerState.InGame);
                            }
                        }                     
                        break;
                    case TargetingReticleType.Road:
                        {
                            if (Physics.Raycast(ray, out hit, float.MaxValue, _groundLayer))
                            {
                                if (_ghost != null)
                                {
                                    _ghost.GetTransform().position = hit.point;
                                }
                                if (_targetingReticle != null)
                                {
                                    if (!_reticleLocked)
                                    {
                                        _targetingReticle.transform.position = hit.point;
                                    }
                                    else
                                    {
                                        Vector3 ignoreY = new Vector3(hit.point.x, _targetingReticle.transform.position.y, hit.point.z);

                                        _targetingReticle.transform.LookAt(ignoreY);
                                    }

                                    if (Input.GetMouseButtonDown(0))
                                    {
                                        if (!_reticleLocked)
                                        {
                                            _reticleLocked = true;
                                            return;
                                        }

                                        Vector3 ignoreY = new Vector3(hit.point.x, _targetingReticle.transform.position.y, hit.point.z);
                                        Vector3 direction = (_targetingReticle.transform.position - ignoreY).normalized;

                                        _ghost = null;
                                        DestroyPreviousTargetingReticle();
                                        PlayerPickerTargetingConfirmed?.Invoke(hit.point, direction);
                                        ChangeState(PlayerPickerState.InGame);
                                    }

                                }
                            }

                            if (Input.GetMouseButtonDown(1))
                            {
                                if (_targetingReticle.activeInHierarchy)
                                {
                                    _targetingReticle.SetActive(false);
                                }
                                _reticleLocked = false;
                                DestroyGhost();
                                PlayerPickerRequestDenied?.Invoke();
                                ChangeState(PlayerPickerState.InGame);
                            }

                            if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                if (_targetingReticle.activeInHierarchy)
                                {
                                    _targetingReticle.SetActive(false);
                                }
                                _reticleLocked = false;
                                DestroyGhost();
                                PlayerPickerRequestDenied?.Invoke();
                                ChangeState(PlayerPickerState.InGame);
                            }
                        }
                        break;
                    default:
                        {
                            DestroyGhost();
                            PlayerPickerRequestDenied?.Invoke();
                            ChangeState(PlayerPickerState.InGame);
                        }                       
                        break;
                }
                //How the picker behaves while Targeting, such as when using an ability.
            }

        }

        public void SetTargetingReticle(TargetingReticle targetingReticle, float width, float length)
        {
            //instantiate targetingReticle.prefab at parent, and then displace it to cursor's raycast position on Update().
            //Also alter the targetingReticle's prefab sizes based on the TargetingReticle's length and width, based on targetingReticle's targetingReticleType.
            if (_targetingReticle != null)
            {
                DestroyPreviousTargetingReticle();
            }
            _targetingReticleDescription = targetingReticle;
            GameObject newTargetingReticle = Instantiate(targetingReticle.Prefab, _targetingParent);
            _targetingReticle = newTargetingReticle;
            switch (targetingReticle.TargetingReticleType)
            {
                case TargetingReticleType.None:
                    DestroyPreviousTargetingReticle();
                    break;
                case TargetingReticleType.Point:
                    //Nothing.
                    break;
                case TargetingReticleType.Area:
                    newTargetingReticle.transform.localScale = new Vector3(width,
                                                                           newTargetingReticle.transform.localScale.y,
                                                                           length);
                    break;
                case TargetingReticleType.Road:
                    newTargetingReticle.transform.localScale = new Vector3(width,
                                                       newTargetingReticle.transform.localScale.y,
                                                       length);
                    break;
                default:
                    DestroyPreviousTargetingReticle();
                    break;
            }
        }

        private void DestroyPreviousTargetingReticle()
        {
            Destroy(_targetingReticle);
        }

		[ContextMenu("Activate")]
		private void DoActivate() => Activate(true);

		[ContextMenu("Deactivate")]
		private void DoDeactivate() => Activate(false);

	}
}