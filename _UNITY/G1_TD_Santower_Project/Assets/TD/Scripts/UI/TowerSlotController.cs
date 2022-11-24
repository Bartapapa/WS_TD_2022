﻿namespace GSGD1
{
	using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	public enum State
	{
		Available = 0,
		GhostVisible
	}

	public class TowerSlotController : MonoBehaviour
	{
		[SerializeField]
		private TowerSlot[] _towerSlots = null;

		[System.NonSerialized]
		private State _state = State.Available;

		[System.NonSerialized]
		private TowerDescription _currentTowerDescription = null;

		public PlayerPickerController PlayerPickerController
		{
			get
			{
				return LevelReferences.Instance.PlayerPickerController;
			}
		}

		private void OnEnable()
		{
			for (int i = 0, length = _towerSlots.Length; i < length; i++)
			{
				_towerSlots[i].OnTowerSlotClicked -= TowerSlotController_OnTowerSlotClicked;
				_towerSlots[i].OnTowerSlotClicked += TowerSlotController_OnTowerSlotClicked;
			}
		}

		private void OnDisable()
		{
			for (int i = 0, length = _towerSlots.Length; i < length; i++)
			{
				_towerSlots[i].OnTowerSlotClicked -= TowerSlotController_OnTowerSlotClicked;
			}
		}

		private void TowerSlotController_OnTowerSlotClicked(TowerSlot sender)
		{
			if (_state == State.Available)
			{
                if (ResourceManager.Instance.CanBuy(ResourceManager.ResourceType.Cookie, sender.TowerDescription.CookieCost))
                {
                    _currentTowerDescription = sender.TowerDescription;
                    ChangeState(State.GhostVisible);
                }
            }
		}

		private void Update()
		{
			if (_state == State.GhostVisible)
			{
				if (Input.GetMouseButtonDown(0) == true)
				{
					if (PlayerPickerController.TrySetGhostAsCellChild() == true || PlayerPickerController.TrySetGhostAsPlateChild() == true)
					{
                        ResourceManager.Instance.AcquireResource(ResourceManager.ResourceType.Cookie, -_currentTowerDescription.CookieCost);
						ChangeState(State.Available);
                        PlayerPickerController.ChangeState(PlayerPickerState.InGame);
                    }
				}
				if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape) == true)
				{
					ChangeState(State.Available);
                    PlayerPickerController.ChangeState(PlayerPickerState.InGame);
                }
			}
		}

		public void ChangeState(State newState)
		{
			switch (newState)
			{
				case State.Available:
				{
					PlayerPickerController.DestroyGhost();
					PlayerPickerController.Activate(false);
					_currentTowerDescription = null;
				}
				break;
				case State.GhostVisible:
				{
                    Tower newTower = _currentTowerDescription.Instantiate(_currentTowerDescription.Prefab);
                    PlayerPickerController.ActivateWithGhost(newTower);
                    newTower.SetTotalCookieCost(_currentTowerDescription.CookieCost);
                }
					break;
				default:
					break;
			}
			_state = newState;
		}
	}
}