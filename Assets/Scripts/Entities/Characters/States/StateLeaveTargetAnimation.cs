using Assets.Scripts.Components.StateMachine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities.Characters
{
	public class StateLeaveTargetAnimation : State<CharacterActionEnum, BaseCharacter>
	{
		public StateLeaveTargetAnimation()
		{
			Entered += OnStateEntered;
		}

		private void OnStateEntered(object sender, System.EventArgs e)
		{
			Entity.StartCoroutine(WaitAnimation());
		}

		private IEnumerator WaitAnimation()
		{
			yield return new WaitForSeconds(0.2f);
			StateMachine.ChangeState(CharacterActionEnum.MoveToHome);
		}
	}
}
