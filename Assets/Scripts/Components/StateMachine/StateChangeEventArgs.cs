using System;

namespace Assets.Scripts.Components.StateMachine
{
	public class StateChangeEventArgs<K, T> : EventArgs where K : IConvertible
	{
		public State<K, T> PreviousState { get; }
		public State<K, T> CurrentState { get; }

		public StateChangeEventArgs(State<K, T> state, State<K, T> prevState)
		{
			CurrentState = state;
			PreviousState = prevState;
		}

		public override string ToString() => $"[StateChangeEventArgs {PreviousState} => {CurrentState}]";
	}
}
