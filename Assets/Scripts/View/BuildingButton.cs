using System;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
	public class BuildingButton : MonoBehaviour
	{
		public BuildingLogo[] LogoBuildings;

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
