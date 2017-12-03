using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Building;

using UnityEngine;

using Random = System.Random;

using Assets.Scripts.Components;
using Assets.Scripts.Components.StateMachine;
using Assets.Scripts.Entities.Characters;
using Assets.Scripts.States;
using Assets.Scripts.View;

using DG.Tweening;

namespace Assets.Scripts
{
	/// <summary>
	/// Game Controller
	/// </summary>
	public class GameController : MonoBehaviour
	{
		public StateMachine<GameStates, GameController> States { get; private set; }

		public CameraController CameraController;
		public GameConfigFactory GameConfig;
		public GameUi GameUi;
		private int houseSpawnedCounter;
		public ResultUi ResultUi;
		public TitleUi TitleUi;
		public event EventHandler<CharacterSpawnedArgs> CharacterSpawned;
		public float Duration => Time.time - firstTime;
		private float firstTime;
		private float lastTime;

		private void Start()
		{
			Init();
			GameUi.Setup(this, GameConfig.Buildings.OrderBy(x => x.Price).ToArray());
			rnd = new Random();
		}

		/// <summary>
		/// Bind events on each building slot
		/// </summary>
		private void Init()
		{
			States = new StateMachine<GameStates, GameController>(this);
			States.Add(GameStates.Loading, new State<GameStates, GameController>());
			States.Add(GameStates.Title, new StateTitle(TitleUi, GameUi));
			States.Add(GameStates.Game, new StateGame());
			States.Add(GameStates.Result, new StateResult(ResultUi, GameUi));

			Money = GameConfig.InitMoney;
			popularity = 1;
			foreach (Slot slot in Slot.Slots)
			{
				slot.SlotClicked += OnSlotClicked;
			}

			firstTime = Time.time;
			lastTime = Time.time;

			States.Start();
			States.ChangeState(GameStates.Title);
		}

		public void UseBuilding(GameBuildingConfig config)
		{
			selectedBuilding = config;
			Debug.Log($"Use Building {config}");
		}

		#region  Money

		private float score;
		public event EventHandler ScoreChanged;

		public float Score
		{
			get
			{
				return score;
			}
			private set
			{
				if (States.Current.Id != GameStates.Game)
				{
					return;
				}
				score = value;
				ScoreChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		#endregion

		#region  Money

		private float money;
		public event EventHandler MoneyChanged;

		public float Money
		{
			get { return money; }
			private set
			{
				money = value;
				MoneyChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		#endregion

		#region  Popularity

		private float popularity;
		public event EventHandler PopularityChanged;

		public float Popularity
		{
			get { return popularity; }
			private set
			{
				popularity = value;
				PopularityChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		#endregion

		#region Buildings

		private static Random rnd = new Random();

		private GameBuildingConfig selectedBuilding;

		#endregion

		#region Spawn / Creation

		private void CreateBuilding(Slot slot, BuildingEnum type)
		{
			BaseBuilding building = slot.Build(type);
			if (building == null)
			{
				return;
			}
			Vector3 original = building.Model.transform.position;

			building.Model.transform.position -= Vector3.up * 5;
			building.Model.DOMove(original, 0.4f);
		}

		private void CreateCharacter(Slot slot, CharacterEnum type)
		{
			BaseCharacter newChar = slot.SpawnCharacter(type);
			newChar.CharacterUpseted += OnCharacterUpseted;
			newChar.InterestReached += OnCharacterInterestReached;
			CharacterSpawned?.Invoke(this, new CharacterSpawnedArgs {Character = newChar});
			Score += 5;
		}

		private void OnCharacterInterestReached(object sender, EventArgs e)
		{
			Money += 15;
			Score += 20;
		}

		private void OnCharacterUpseted(object sender, EventArgs eventArgs)
		{
			Popularity = Math.Max(Popularity - 0.1f, 0);
			if (Popularity <= 0.01f)
			{
				(States.States[GameStates.Game] as StateGame)?.GameOver();
			}
			Score -= 10;
		}

		#endregion

		#region Interaction

		private Transform lastHouse;

		private void OnSlotClicked(object sender, EventArgs e)
		{
			var slot = sender as Slot;
			if (slot == null)
			{
				throw new Exception("da fuck");
			}

			if (selectedBuilding == null)
			{
				return;
			}

			if (selectedBuilding.Price > Money)
			{
				return;
			}

			Money -= selectedBuilding.Price;

			CreateBuilding(slot, selectedBuilding.Type);
		}

		public void SpawnHouse()
		{
			Slot slot = null;

			// pick a random slot @ proximity
			if (lastHouse != null)
			{
				List<Slot> slots = Slot.Slots
										.Where(x => x.Content == null)
										.Where(x => Vector3.Distance(x.transform.position, lastHouse.transform.position) < 22f).ToList();
				if (slots.Count > 0)
				{
					int r = rnd.Next(slots.Count);
					slot = slots[r];
				}
			}

			// pick a random slot
			if (slot == null)
			{
				var slots = Slot.Slots.Where(x => x.Content == null).ToList();
				int r = rnd.Next(slots.Count);
				slot = slots[r];
			}

			if (slot == null)
			{
				return;
			}

			CreateBuilding(slot, BuildingEnum.House);

			if (houseSpawnedCounter < 3)
			{
				CreateCharacter(slot, CharacterEnum.Girl);
				CameraController.MoveTo(slot.transform.position);
			}
			else
			{
				var type = CharacterEnum.Boy;
				switch (UnityEngine.Random.Range(0, 4))
				{
					case 1:
						type = CharacterEnum.Boy2;
						break;
					case 2:
						type = CharacterEnum.Girl;
						break;
					case 3:
						type = CharacterEnum.Girl2;
						break;
				}
				CreateCharacter(slot, type);
			}


			houseSpawnedCounter++;
			lastHouse = slot.transform;
		}

		#endregion
		private void Update()
		{
			lastTime += Time.deltaTime;
			if (lastTime > 2)
			{
				lastTime -= 2;
				Score += 2;
			}
		}

	}

	public class CharacterSpawnedArgs
	{
		public BaseCharacter Character;
	}
}
