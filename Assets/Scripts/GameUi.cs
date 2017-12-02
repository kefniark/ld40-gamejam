using System.Collections.Generic;

using Assets.Scripts.View;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class GameUi : MonoBehaviour
	{
		public Image PopularityFill;
		public Text Money;

		public GameController Game { get; private set; }
		
		private readonly List<Button> BuildingButtons = new List<Button>();

		internal void Setup(GameController gameController, GameBuildingConfig[] buildings)
		{
			Game = gameController;
			
			foreach (GameBuildingConfig building in buildings)
			{
				InstantiateButton(building);
			}

			Game.MoneyChanged += (sender, args) => UpdateMoney();
			Game.PopularityChanged += (sender, args) => UpdatePopularity();
			Game.CharacterSpawned += (sender, args) => InstantiateCharacterInfoUi(args);
			UpdateMoney();
			UpdatePopularity();
		}

		#region Instantiate Character 

		public Transform CharacterInfoContainer;
		public GameObject CharacterInfoPrefab;

		private void InstantiateCharacterInfoUi(CharacterSpawnedArgs args)
		{
			GameObject go = GameObject.Instantiate(CharacterInfoPrefab);
			go.transform.SetParent(CharacterInfoContainer, false);
			go.GetComponent<CharacterInfoUi>()?.Setup(args.Character);
		}

		#endregion

		#region Instantiate Buttons

		public Transform BuildingButtonContainer;
		public GameObject BuildingButtonPrefab;

		private void InstantiateButton(GameBuildingConfig building)
		{
			GameObject go = GameObject.Instantiate(BuildingButtonPrefab);
			go.transform.SetParent(BuildingButtonContainer, false);
			var text = go.GetComponentInChildren<Text>();
			text.text = $"{building.Type}\n\n{building.Price}$";
			var button = go.GetComponentInChildren<Button>();
			button.onClick.AddListener(() => OnClickBuildingButton(button, building));
			BuildingButtons.Add(button);
		}

		#endregion

		private void OnClickBuildingButton(Button clickedButton, GameBuildingConfig config)
		{
			Game.UseBuilding(config);
			foreach (Button button in BuildingButtons)
			{
				button.interactable = button != clickedButton;
			}
		}

		private void UpdateMoney() => Money.text = Game.Money + " $";

		private void UpdatePopularity() => PopularityFill.DOFillAmount(Game.Popularity, 0.25f);
	}
}
