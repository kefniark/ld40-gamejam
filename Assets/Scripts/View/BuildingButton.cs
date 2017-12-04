using System;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
	public class BuildingButton : MonoBehaviour
	{
		public AudioSource ClickSfx;
		public BuildingLogo[] LogoBuildings;

		private void Awake()
		{
			GetComponentInChildren<Button>().onClick.AddListener(() => ClickSfx.Play());
		}

		public void ShowLogo(BuildingEnum val)
		{
			foreach (BuildingLogo logo in LogoBuildings)
			{
				logo.Logo.gameObject.SetActive(logo.Type == val);
			}
		}
	}

	[Serializable]
	public class BuildingLogo
	{
		public BuildingEnum Type;
		public Image Logo;
	}
}
