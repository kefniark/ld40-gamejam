using Assets.Scripts.Components.StateMachine;

using DG.Tweening;

namespace Assets.Scripts.Entities.Characters
{
	public class StateLeaveAnimation : State<CharacterActionEnum, BaseCharacter>
	{
		public StateLeaveAnimation()
		{
			Entered += OnStateEntered;
		}

		private void OnStateEntered(object sender, System.EventArgs e)
		{
			Entity.transform.DOMove(Entity.Origin.Door.position, 0.8f).OnComplete(() =>
			{
				Entity.NavMeshAgent.enabled = true;
				StateMachine.ChangeState(CharacterActionEnum.Decide);
			});
		}
	}
}
