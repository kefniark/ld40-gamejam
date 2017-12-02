using Assets.Scripts.Components.StateMachine;

namespace Assets.Scripts.Entities.Characters
{
	public class StateMoveToHome : State<CharacterActionEnum, BaseCharacter>
	{
		public StateMoveToHome()
		{
			Entered += OnStateEntered;
		}

		private void OnStateEntered(object sender, System.EventArgs e)
		{
			Entity.GoToHome();
		}

		public override void Update()
		{
			if (!Entity.AgentHasReachedDestination())
			{
				return;
			}

			StateMachine.ChangeState(CharacterActionEnum.EnterAnimation);
		}
	}
}
