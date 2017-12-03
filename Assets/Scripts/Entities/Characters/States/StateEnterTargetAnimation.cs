using Assets.Scripts.Components.StateMachine;

using DG.Tweening;

using UnityEngine;

namespace Assets.Scripts.Entities.Characters
{
	public class StateEnterTargetAnimation : State<CharacterActionEnum, BaseCharacter>
	{
		public StateEnterTargetAnimation()
		{
			Entered += OnStateEntered;
		}

		private void OnStateEntered(object sender, System.EventArgs e)
		{
			Transform target = Entity.Target.transform;
			Entity.NavMeshAgent.enabled = false;
			Entity.transform.DOMove(target.position, 0.8f).OnComplete(() => {
				Entity.SolvedInterest();
				StateMachine.ChangeState(CharacterActionEnum.WaitInsideTarget);
			});
		}
	}
}
