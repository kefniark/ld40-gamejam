using Assets.Scripts.Components.StateMachine;
using Assets.Scripts.Entities.Characters;

using DG.Tweening;

using UnityEngine;

namespace Assets.Scripts.Components
{
	[ExecuteInEditMode]
	public class WorldOffscreenTrackingUi : MonoBehaviour
	{
		private CanvasGroup canvas;
		private bool display;
		private Transform followThis;
		private Tweener tween;
		private bool waiting;

		public void Setup(Transform follow, BaseCharacter character)
		{
			followThis = follow;
			State<CharacterActionEnum, BaseCharacter> wait = character.States.States[CharacterActionEnum.WaitTarget];
			wait.Entered += (sender, args) =>
			{
				waiting = true;
				Show();
			};
			wait.Exited += (sender, args) =>
			{
				waiting = false;
				Hide();
			};
			canvas = GetComponent<CanvasGroup>();
			canvas.alpha = 0;
		}

		private void Update()
		{
			if (followThis == null)
			{
				return;
			}

			if (!waiting)
			{
				return;
			}

			Vector2 sp = Camera.main.WorldToScreenPoint(followThis.position);
			var center = new Vector2(Screen.width / 2f, Screen.height / 2f);

			Vector2 direction = sp - center;

			float angle = Mathf.Atan2(direction.x, direction.y);
			var bounds = new Vector2(Screen.width / 2f - 40, Screen.height / 2f - 40);

			var isBorder = false;

			Vector2 realPosition = center + direction;
			if (direction.x > bounds.x)
			{
				realPosition.x = center.x + bounds.x - 120;
				isBorder = true;
			}
			if (direction.x < -bounds.x)
			{
				realPosition.x = center.x - bounds.x + 120;
				isBorder = true;
			}

			if (direction.y > bounds.y)
			{
				realPosition.y = center.y + bounds.y - 20;
				isBorder = true;
			}
			if (direction.y < -bounds.y)
			{
				realPosition.y = center.y - bounds.y;
				isBorder = true;
			}

			if (isBorder && !display)
			{
				Show();
			}
			else if (!isBorder && display)
			{
				Hide();
				return;
			}

			transform.position = realPosition;
			transform.localEulerAngles = new Vector3(0, 0, -angle * Mathf.Rad2Deg);
		}

		private void Hide()
		{
			tween?.Kill();
			tween = canvas.DOFade(0, 0.2f);
			display = false;
		}

		private void Show()
		{
			tween?.Kill();
			tween = canvas.DOFade(1, 0.2f);
			display = true;
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
