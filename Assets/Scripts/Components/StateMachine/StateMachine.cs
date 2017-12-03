using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Components.StateMachine
{
	public class StateMachine<K, T> where K : IConvertible
	{
		public Dictionary<K, State<K, T>> States { get; } = new Dictionary<K, State<K, T>>();
		public State<K, T> Current { get; private set; }
		public State<K, T> Previous { get; private set; }
		public readonly T Parent;

		public StateMachine(T parent)
		{
			Parent = parent;
		}

		public event EventHandler<StateChangeEventArgs<K, T>> StateChanged;

		public void Add(K id, State<K, T> state)
		{
			States.Add(id, state);
			state.Init(id, this, Parent);
		}

		public void Remove(K id) => States.Remove(id);

		public void ChangeState(K id)
		{
			if (Current == null)
			{
				throw new Exception($"{this} This stateMachine was not started");
			}

			if (!States.ContainsKey(id))
			{
				throw new Exception($"{this} This state doesn\'t exist {id}");
			}

			ChangeState(States[id]);
		}

		private void ChangeState(State<K, T> state)
		{
			if (state == Current)
			{
				UnityEngine.Debug.LogWarning($"{this} Can\'t change to the same state");
				return;
			}

			if (Current != null)
			{
				Current.Exit();
				Previous = Current;
			}

			Current = state;

			StateChanged?.Invoke(this, new StateChangeEventArgs<K, T>(Current, Previous));

			Current.Enter();
		}

		public void Start()
		{
			if (Current != null)
			{
				UnityEngine.Debug.LogWarning($"{this} Can\'t start, seems already started");
				return;
			}

			KeyValuePair<K, State<K, T>> state = States.FirstOrDefault();
			if (state.Value == null)
			{
				throw new Exception($"{this} No default state, cannot start");
			}

			// TODO: Check if these need to be switched over (CC: Almir bug)?
			state.Value.Enter();
			Current = state.Value;
		}

		public void Back()
		{
			if (Previous == null)
			{
				return;
			}
			ChangeState(Previous);
			Previous = null;
		}

		public void Exit()
		{
			if (Current == null)
			{
				UnityEngine.Debug.LogWarning($"{this} Can\'t exit, seems already exited");
				return;
			}
			Current.Exit();
			Current = null;
			Previous = null;
		}

		public void Update()
		{
			Current?.Update();
		}

		public override string ToString() => $"[StateMachine of {Parent}]";
	}
}
