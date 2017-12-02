using Assets.Scripts.Components;
using Assets.Scripts.Entities.Characters;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
	public class CharacterInfoUi : MonoBehaviour
	{
		public WorldObjectTrackingUi Tracking;
		public BaseCharacter Character { get; private set; }
		public Image WaitFill;
		public Text Target;
		private CanvasGroup Canvas;

		public void Setup(BaseCharacter character)
		{
			Character = character;
			Tracking.FollowThis = Character.transform;
			Canvas = GetComponent<CanvasGroup>();
			Character.WaitChanged += OnWaitChanged;
			Character.InterestChanged += (obj, args) => UpdateInterest();
			UpdateInterest();
		}

		private void OnWaitChanged(object sender, System.EventArgs e)
		{
			WaitFill.fillAmount = Character.Wait;
		}

		private void UpdateInterest()
		{
			if (Character.Interest == null)
			{
				Target.text = "";
				return;
			}
			Target.text = Character.Interest.Type.ToString();
		}
	}
}
