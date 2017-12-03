using Assets.Scripts.Components.StateMachine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities.Characters
{
	public class StateWaitInsideTarget : State<CharacterActionEnum, BaseCharacter>
	{
		public StateWaitInsideTarget()
		{
			Entered += OnStateEntered;
		}

		private void OnStateEntered(object sender, System.EventArgs e)
		{
			Entity.StartCoroutine(WaitAnimation());
		}

		private IEnumerator WaitAnimation()
		{
			yield return new WaitForSeconds(1f);
			StateMachine.ChangeState(CharacterActionEnum.LeaveTargetAnimation);
		}
	}
}
