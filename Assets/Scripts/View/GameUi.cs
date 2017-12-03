using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
	public class GameUi : MonoBehaviour
	{
		public GameController Game { get; private set; }

		private readonly List<Button> BuildingButtons = new List<Button>();
		public Text Money;
		public Text Score;
		public Image PopularityFill;

		internal void Setup(GameController gameController, GameBuildingConfig[] buildings)
		{
			Game = gameController;

			foreach (GameBuildingConfig building in buildings)
			{
				InstantiateButton(building);
			}

			Game.MoneyChanged += (sender, args) => UpdateMoney();
			Game.ScoreChanged += (sender, args) => UpdateScore();
			Game.PopularityChanged += (sender, args) => UpdatePopularity();
			Game.CharacterSpawned += (sender, args) => InstantiateCharacterInfoUi(args);
			UpdateScore();
			UpdateMoney();
			UpdatePopularity();
		}

		private void OnClickBuildingButton(Button clickedButton, GameBuildingConfig config)
		{
			Game.UseBuilding(config);
			foreach (Button button in BuildingButtons)
			{
				button.interactable = button != clickedButton;
			}
		}

		private void UpdateScore() => Score.text = $"{Game.Score:000000}";

		private void UpdateMoney() => Money.text = Game.Money + " K$";

		private void UpdatePopularity() => PopularityFill.DOFillAmount(Game.Popularity, 0.25f);

		#region Instantiate Character 

		public Transform CharacterInfoContainer;
		public GameObject CharacterInfoPrefab;

		private void InstantiateCharacterInfoUi(CharacterSpawnedArgs args)
		{
			GameObject go = Instantiate(CharacterInfoPrefab);
			go.transform.SetParent(CharacterInfoContainer, false);
			go.GetComponent<CharacterInfoUi>()?.Setup(args.Character);
		}

		#endregion

		#region Instantiate Buttons

		public Transform BuildingButtonContainer;
		public GameObject BuildingButtonPrefab;

		private void InstantiateButton(GameBuildingConfig building)
		{
			GameObject go = Instantiate(BuildingButtonPrefab);
			go.transform.SetParent(BuildingButtonContainer, false);

			var buildingButton = go.GetComponentInChildren<BuildingButton>();
			buildingButton.ShowLogo(building.Type);
			var text = go.GetComponentInChildren<Text>();
			text.text = $"{building.Price}K$";
			var button = go.GetComponentInChildren<Button>();
			button.onClick.AddListener(() => OnClickBuildingButton(button, building));
			BuildingButtons.Add(button);
		}

		#endregion
	}
}
