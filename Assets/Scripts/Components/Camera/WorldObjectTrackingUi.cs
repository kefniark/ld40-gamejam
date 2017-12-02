using UnityEngine;

namespace Assets.Scripts.Components
{
	[ExecuteInEditMode]
	public class WorldObjectTrackingUi : MonoBehaviour
	{
		public Transform FollowThis;
		public Vector2 Offset;

		private void Update()
		{
			if (FollowThis == null)
			{
				return;
			}

			Vector2 sp = Camera.main.WorldToScreenPoint(FollowThis.position);

			transform.position = sp + Offset;
		}

		private void OnEnable()
		{
			if (gameObject.activeInHierarchy)
			{
				Update();
			}
		}
	}
}
