using Assets.Scripts.Components.StateMachine;
using System.Collections;

using UnityEngine;

namespace Assets.Scripts.Entities.Characters
{
	public class StateUpset : State<CharacterActionEnum, BaseCharacter>
	{
		public StateUpset()
		{
			Entered += OnStateEntered;
		}

		private void OnStateEntered(object sender, System.EventArgs e)
		{
			Entity.StartCoroutine(WaitAnimation());
		}

		private IEnumerator WaitAnimation()
		{
			yield return new WaitForSeconds(2f);
			StateMachine.ChangeState(CharacterActionEnum.Decide);
		}
	}
}
