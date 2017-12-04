using System.Collections.Generic;

using Assets.Scripts.Components;

using DG.Tweening;

using UnityEngine;
using UnityEngine.Audio;
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

		public AudioSource StartSfx;
		public AudioSource GameOverSfx;
		public AudioSource MoneySfx;
		public AudioSource ComplainSfx;
		public AudioMixer AudioSfx;

		public void Awake() => AudioSfx.DOSetFloat("SfxVolume", 0, 0.1f);

		internal void Setup(GameController gameController, GameBuildingConfig[] buildings)
		{
			Game = gameController;

			foreach (GameBuildingConfig building in buildings)
			{
				InstantiateButton(building);
			}

			Game.States.States[GameStates.Game].Entered += (sender, args) => StartSfx.PlayDelayed(1);
			Game.States.States[GameStates.Game].Exited += (sender, args) => GameOverSfx.PlayDelayed(1);
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

		private void UpdateMoney() => Money.text = $"{Game.Money} K$";

		private void UpdatePopularity() => PopularityFill.DOFillAmount(Game.Popularity, 0.25f);

		#region Instantiate Character 

		public Transform CharacterInfoContainer;
		public GameObject CharacterInfoPrefab;
		public GameObject CharacterArrowPrefab;

		private void InstantiateCharacterInfoUi(CharacterSpawnedArgs args)
		{
			GameObject go = Instantiate(CharacterInfoPrefab);
			go.transform.SetParent(CharacterInfoContainer, false);
			go.GetComponent<CharacterInfoUi>()?.Setup(args.Character);

			GameObject go2 = Instantiate(CharacterArrowPrefab);
			go2.transform.SetParent(CharacterInfoContainer, false);
			go2.GetComponent<WorldOffscreenTrackingUi>()?.Setup(args.Character.transform, args.Character);

			args.Character.States.States[CharacterActionEnum.WaitInsideTarget].Entered += (sender, arg2) => MoneySfx.Play();
			args.Character.States.States[CharacterActionEnum.Upsetted].Entered += (sender, arg2) => ComplainSfx.Play();
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
