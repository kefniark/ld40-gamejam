using Assets.Scripts.Components.StateMachine;

namespace Assets.Scripts.Entities.Characters
{
	public class StateDecide : State<CharacterActionEnum, BaseCharacter>
	{
		public StateDecide()
		{
			Entered += OnStateEntered;
		}

		private void OnStateEntered(object sender, System.EventArgs e)
		{
			Entity.DecideInterest();
			StateMachine.ChangeState(CharacterActionEnum.WaitTarget);
		}
	}
}
