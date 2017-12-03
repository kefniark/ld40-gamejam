using System;

using Assets.Scripts.Components;
using Assets.Scripts.Entities.Characters;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
	public class CharacterInfoUi : MonoBehaviour
	{
		public CharacterInfoLogo[] LogoActions;
		public WorldObjectTrackingUi Tracking;
		public BaseCharacter Character { get; private set; }
		public Image Wait;
		public Image WaitFill;
		
		public void Setup(BaseCharacter character)
		{
			Character = character;
			Tracking.FollowThis = Character.transform;

			var waitTarget = Character.States.States[CharacterActionEnum.WaitTarget] as StateWaitTarget;
			waitTarget.Entered += (sender, arg) => ShowWait();
			waitTarget.WaitChanged += (sender, arg) => WaitFill.fillAmount = waitTarget.Wait;
			waitTarget.Exited += (sender, arg) => HideWait();

			Character.States.States[CharacterActionEnum.MoveToTarget].Entered += (sender, arg) => ShowLogo("walk");
			Character.States.States[CharacterActionEnum.MoveToHome].Entered += (sender, arg) => ShowLogo("House");
			Character.States.States[CharacterActionEnum.EnterTargetAnimation].Entered += (sender, arg) => ShowLogo("happy");

			Character.InterestChanged += (obj, args) => UpdateInterest();
			UpdateInterest();
			HideWait();

			var canvas = GetComponent<CanvasGroup>();
			canvas.DOFade(1, 0.5f).SetDelay(2.5f);
		}

		private void UpdateInterest()
		{
			if (Character.Interest == null)
			{
				ShowLogo("");
				return;
			}
			ShowLogo(Character.Interest.Type.ToString());
		}

		private void HideWait()
		{
			Wait.gameObject.SetActive(false);
		}

		private void ShowWait()
		{
			WaitFill.fillAmount = 0;
			Wait.gameObject.SetActive(true);
		}

		private void ShowLogo(string val)
		{
			foreach (CharacterInfoLogo logo in LogoActions)
			{
				logo.Logo.gameObject.SetActive(logo.Type == val);
			}
		}
	}

	[Serializable]
	public class CharacterInfoLogo
	{
		public string Type;
		public Image Logo;
	}
}
