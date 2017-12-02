using Assets.Scripts.Components.StateMachine;

namespace Assets.Scripts.Entities.Characters
{
	public class StateMoveToTarget : State<CharacterActionEnum, BaseCharacter>
	{
		public override void Update()
		{
			if (!Entity.AgentHasReachedDestination())
			{
				return;
			}

			StateMachine.ChangeState(CharacterActionEnum.EnterTargetAnimation);
		}
	}
}
