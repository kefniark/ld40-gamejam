using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Building;
using Assets.Scripts.Entities.Characters;

using UnityEngine;

using Random = System.Random;
using Assets.Scripts.Components;

namespace Assets.Scripts
{
	/// <summary>
	/// Game Controller
	/// </summary>
	public class GameController : MonoBehaviour
	{
		public event EventHandler<CharacterSpawnedArgs> CharacterSpawned;
		#region  Money

		private float money;
		public event EventHandler MoneyChanged;

		public float Money
		{
			get
			{
				return money;
			}
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
			get
			{
				return popularity;
			}
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
		
		public CameraController CameraController;
		public GameConfigFactory GameConfig;
		public GameUi GameUi;
		private float wait = 12f;
		
		private IEnumerator Start()
		{
			Init();
			GameUi.Setup(this, GameConfig.Buildings);
			rnd = new Random();

			while (Popularity > 0.01f)
			{
				SpawnHouse();

				yield return new WaitForSeconds(wait);
				wait = Math.Max(wait - 1, 2);
			}
		}

		/// <summary>
		/// Bind events on each building slot
		/// </summary>
		private void Init()
		{
			Money = GameConfig.InitMoney;
			popularity = 1;
			foreach (Slot slot in Slot.Slots)
			{
				slot.SlotClicked += OnSlotClicked;
			}
		}

		public void UseBuilding(GameBuildingConfig config)
		{
			selectedBuilding = config;
			Debug.Log($"Use Building {config}");
		}

		#region Spawn / Creation

		private void CreateBuilding(Slot slot, BuildingEnum type)
		{
			BaseBuilding newBuilding = slot.Build(type);
			// Debug.Log($"New Buidling Created : {newBuilding}");
		}

		private void CreateCharacter(Slot slot, CharacterEnum type)
		{
			BaseCharacter newChar = slot.SpawnCharacter(type);
			newChar.CharacterUpseted += OnCharacterUpseted;
			newChar.InterestReached += OnCharacterInterestReached;
			CharacterSpawned?.Invoke(this, new CharacterSpawnedArgs {Character = newChar });
			// Debug.Log($"New Character Created : {newChar}");
		}

		private void OnCharacterInterestReached(object sender, EventArgs e)
		{
			Money += 15;
		}

		private void OnCharacterUpseted(object sender, EventArgs eventArgs)
		{
			Popularity = Math.Max(Popularity - 0.1f, 0);
			Debug.Log($"OnCharacterUpseted {Popularity}");
			if (Popularity <= 0.01f)
			{
				Debug.LogError("GameOver");
			}
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

		private void SpawnHouse()
		{
			Slot slot = null;

			// pick a random slot @ proximity
			if (lastHouse != null)
			{
				List<Slot> slots = Slot.Slots
					.Where(x => x.Content == null)
					.Where(x => Vector3.Distance(x.transform.position, lastHouse.transform.position) < 30f).ToList();
				if (slots.Count > 0)
				{
					int r = rnd.Next(slots.Count);
					slot = slots[r];
				}
			}

			// pick a random slot
			if (slot == null)
			{
				int r = rnd.Next(Slot.Slots.Count);
				slot = Slot.Slots[r];
			}

			if (slot == null)
			{
				return;
			}
			
			CreateBuilding(slot, BuildingEnum.House);
			
			CreateCharacter(slot, UnityEngine.Random.Range(0f, 1f) > 0.6 ? CharacterEnum.Boy : CharacterEnum.Girl);
			if (wait >= 10)
			{
				CameraController.MoveTo(slot.transform.position);
			}
			lastHouse = slot.transform;
		}

		#endregion
	}

	public class CharacterSpawnedArgs
	{
		public BaseCharacter Character;
	}
}
