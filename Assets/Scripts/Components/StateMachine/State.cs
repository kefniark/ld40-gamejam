using System;

namespace Assets.Scripts.Components.StateMachine
{
	public class State<K, T> where K : IConvertible
	{
		// properties
		public K Id { get; private set; }

		public bool IsActive { get; private set; }
		public StateMachine<K, T> StateMachine { get; private set; }

		public T Entity { get; private set; }

		// events
		public event EventHandler Entered;

		public event EventHandler Exited;

		/// <summary>
		/// Used by the stateMachine to setup this state
		/// </summary>
		/// <param name="id"></param>
		/// <param name="statemachine"></param>
		/// <param name="entity"></param>
		public void Init(K id, StateMachine<K, T> statemachine, T entity)
		{
			Id = id;
			StateMachine = statemachine;
			Entity = entity;
		}

		/// <summary>
		/// Enter state
		/// </summary>
		public void Enter()
		{
			IsActive = true;
			Entered?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Exit state
		/// </summary>
		public void Exit()
		{
			IsActive = false;
			Exited?.Invoke(this, EventArgs.Empty);
		}

		public virtual void Update() { }

		public override string ToString() => StateMachine == null ? "[State]" : $"[State of {StateMachine.Parent} : {Id} - {IsActive}]";
	}
}
