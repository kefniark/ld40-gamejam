using Assets.Scripts.Components.StateMachine;

using DG.Tweening;

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
			Transform target = Entity.Target.Door.transform;
			Entity.transform.DOMove(target.position, 0.8f).OnComplete(() => {
				Entity.NavMeshAgent.enabled = true;
				StateMachine.ChangeState(CharacterActionEnum.MoveToHome);
			});
		}
	}
}
