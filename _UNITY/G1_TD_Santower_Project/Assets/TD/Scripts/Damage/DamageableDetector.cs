namespace GSGD1
{
	using System.Collections.Generic;
	using UnityEngine;

	public class DamageableDetector : MonoBehaviour
	{
		public enum Mode
		{
			firstEnter,
			nearest,
			highestHealth,
			lowestHealth
		}

		[SerializeField]
		private Mode _mode = Mode.firstEnter;

		[SerializeField]
		private bool _targetFlyingEnemies = false;

		[SerializeField]
		private bool _canOnlyTargetFlyingEnemies = false;

		[SerializeField]
		private List<Damageable> _damageablesInRange = new List<Damageable>();

		public List<Damageable> DamageablesInRange => _damageablesInRange;

		public Damageable GetTarget()
		{
			switch (_mode)
			{
				case Mode.firstEnter:
					return GetFirstDamageable();
				case Mode.nearest:
					return GetNearestDamageable();
				case Mode.highestHealth:
					return GetHighestHealthDamageable();
				case Mode.lowestHealth:
					return GetLowestHealthDamageable();
				default:
					return default;
			}
		}

		public bool HasAnyDamageableInRange()
		{
			RemoveNullItemsFromList();
			return _damageablesInRange.Count > 0;
		}

		public Damageable GetFirstDamageable()
		{
			if (HasAnyDamageableInRange() == true)
			{
				return _damageablesInRange[0];
			}
			else
			{
				return null;
			}
		}	
		public Damageable GetSecondDamageable()
		{
			if (HasAnyDamageableInRange() == true && _damageablesInRange.Count > 1)
			{
				return _damageablesInRange[1];
			}
			else if (HasAnyDamageableInRange() == true && _damageablesInRange.Count == 1)
			{
				return _damageablesInRange[0];
			}
			else
			{
				return null;
			}
		}

		public Damageable GetNearestDamageable()
		{
			float shortestDistance = 0;
			int shortestDistanceIndex = 0;
			for (int i = 0, length = _damageablesInRange.Count; i < length; i++)
			{
				var distance = (_damageablesInRange[i].transform.position - transform.position).sqrMagnitude;
				if (distance < shortestDistance)
				{
					shortestDistance = distance;
					shortestDistanceIndex = i;
				}
			}
			return _damageablesInRange[shortestDistanceIndex];
		}

		public Damageable GetLowestHealthDamageable()
		{
			float lowestHealth = 0;
			int lowestHealthIndex = 0;
			for (int i = 0, length = _damageablesInRange.Count; i < length; i++)
			{ 
				if (_damageablesInRange[i].GetHealth < lowestHealth)
				{
					lowestHealth = _damageablesInRange[i].GetHealth;
					lowestHealthIndex = i;
				}
			}
			return _damageablesInRange[lowestHealthIndex];
		}

		public Damageable GetHighestHealthDamageable()
		{
			float highestHealth = 0;
			int highestHealthIndex = 0;
			for (int i = 0, length = _damageablesInRange.Count; i < length; i++)
			{
				if (_damageablesInRange[i].GetHealth > highestHealth)
				{
					highestHealth = _damageablesInRange[i].GetHealth;
					highestHealthIndex = i;
				}
			}
			return _damageablesInRange[highestHealthIndex];
		}

		public void RemoveNullItemsFromList()
		{
            for (var i = _damageablesInRange.Count - 1; i > -1; i--)
            {
                if (_damageablesInRange[i] == null)
				{
                    _damageablesInRange.RemoveAt(i);
                }   
            }
        }


		private void OnTriggerEnter(Collider other)
		{
			Damageable damageable = other.GetComponentInParent<Damageable>();
			if (damageable != null && _damageablesInRange.Contains(damageable) == false && ((damageable.GetIsFlying == _canOnlyTargetFlyingEnemies) || (_targetFlyingEnemies == true)))
			{
				damageable.DamageTaken -= Damageable_OnDamageTaken;
				damageable.DamageTaken += Damageable_OnDamageTaken;

				damageable.CallerDied -= Damageable_OnCallerDied;
				damageable.CallerDied += Damageable_OnCallerDied;
				_damageablesInRange.Add(damageable);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			Damageable damageable = other.GetComponentInParent<Damageable>();

			if (damageable != null && _damageablesInRange.Contains(damageable) == true && ((damageable.GetIsFlying == _canOnlyTargetFlyingEnemies) || (_targetFlyingEnemies == true)))
			{
				damageable.DamageTaken -= Damageable_OnDamageTaken;
                damageable.CallerDied -= Damageable_OnCallerDied;
                _damageablesInRange.Remove(damageable);
			}
		}

		private void Damageable_OnDamageTaken(Damageable caller, int currentHealth, int damageTaken)
		{
			//if (!_damageablesInRange.Contains(caller)) return;

			//if (currentHealth <= 0)
			//{
			//	_damageablesInRange.Remove(caller);
			//}
		}

		private void Damageable_OnCallerDied(Damageable caller, int currentHealth, int damageTaken)
		{
            if (!_damageablesInRange.Contains(caller)) return;

            _damageablesInRange.Remove(caller);
		}

	}
}