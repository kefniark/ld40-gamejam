using Assets.Scripts.Components.StateMachine;

using DG.Tweening;

namespace Assets.Scripts.Entities.Characters
{
	public class StateEnterAnimation : State<CharacterActionEnum, BaseCharacter>
	{
		public StateEnterAnimation()
		{
			Entered += OnStateEntered;
		}

		private void OnStateEntered(object sender, System.EventArgs e)
		{
			Entity.NavMeshAgent.enabled = false;
			Entity.transform.DOMove(Entity.Origin.transform.position, 0.8f).OnComplete(() => {
				StateMachine.ChangeState(CharacterActionEnum.WaitInside);
			});
		}
	}
}
