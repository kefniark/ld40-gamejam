using System;

namespace Assets.Scripts.Components.StateMachine
{
	public class State<K, T> where K : IConvertible
	{
		public K Id { get; private set; }
		public bool IsActive { get; private set; }
		public StateMachine<K, T> StateMachine { get; private set; }
		public T Entity { get; private set; }
		public event EventHandler Entered;
		public event EventHandler Exited;

		public void Init(K id, StateMachine<K, T> statemachine, T entity)
		{
			Id = id;
			StateMachine = statemachine;
			Entity = entity;
		}

		public void Enter()
		{
			IsActive = true;
			Entered?.Invoke(this, EventArgs.Empty);
		}

		public void Exit()
		{
			IsActive = false;
			Exited?.Invoke(this, EventArgs.Empty);
		}

		public virtual void Update() { }

		public override string ToString() => StateMachine == null ? "[State]" : $"[State of {StateMachine.Parent} : {Id} - {IsActive}]";
	}
}
