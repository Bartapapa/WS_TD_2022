namespace GSGD1
{
	using UnityEngine;

	public interface IPlateChild
	{
		Transform GetTransform();
		void OnSetChild();
	}

	public class Plate : MonoBehaviour
	{
		private IPlateChild _towerChild = null;

		public bool HasChild
		{
			get
			{
				return _towerChild != null;
			}
		}

		public bool SetChild(IPlateChild plateChild)
		{
			if (plateChild == null)
			{
				return false;
			}
			var childTransform = plateChild.GetTransform();
			childTransform.SetParent(transform);
			plateChild.OnSetChild();
			_towerChild = plateChild;

			return true;
		}
	}
}