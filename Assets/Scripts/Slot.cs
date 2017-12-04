using System;
using System.Collections.Generic;

using Assets.Scripts.Building;
using Assets.Scripts.Entities.Characters;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
	/// <summary>
	/// Slot on the map (position where you a building can be buildt)
	/// </summary>
	public class Slot : MonoBehaviour
	{
		public static List<Slot> Slots { get; private set; } = new List<Slot>();

		public event EventHandler SlotClicked;

		[Header("Factories")]
		public BuildingFactory BuildingFactory;
		public CharacterFactory CharacterFactory;

		[Header("Objects")]
		public GameObject Content;
		public BaseBuilding Building;

		[Header("Properties")]
		private Material DefaultColor;
		public Material ErrorColor;
		public Material HoverColor;

		private Renderer CurrentRenderer;
		private MeshRenderer MeshRenderer;

		public bool IsInteractable = true;

		public bool IsSelected { get; private set; }

		private void OnEnable() => Slots.Add(this);

		private void OnDisable() => Slots.Remove(this);

		private void Start()
		{
			CurrentRenderer = GetComponent<Renderer>();
			MeshRenderer = GetComponent<MeshRenderer>();
			MeshRenderer.enabled = false;
		}

		private void GetDefault()
		{
			if (DefaultColor != null)
			{
				return;
			}
			DefaultColor = CurrentRenderer.sharedMaterial;
		}

		private void OnMouseEnter()
		{
			if (!IsInteractable || EventSystem.current.IsPointerOverGameObject())
			{
				return;
			}

			GetDefault();
			
			if (Content != null)
			{
				// CurrentRenderer.material = ErrorColor;
				return;
			}

			IsSelected = true;
			MeshRenderer.enabled = true;
			CurrentRenderer.material = HoverColor;
		}

		private void OnMouseDown()
		{
			if (!IsInteractable || EventSystem.current.IsPointerOverGameObject())
			{
				return;
			}
			SlotClicked?.Invoke(this, EventArgs.Empty);
		}

		private void OnMouseExit()
		{
			if (!IsInteractable)
			{
				return;
			}
			IsSelected = false;
			MeshRenderer.enabled = false;
			CurrentRenderer.material = DefaultColor;
		}

		public BaseBuilding Build(BuildingEnum type)
		{
			if (Content != null)
			{
				return null;
			}

			Content = Instantiate(BuildingFactory.GetPrefab(type), transform);
			Building = Content.GetComponent<BaseBuilding>();
			return Building;
		}

		public BaseCharacter SpawnCharacter(CharacterEnum type)
		{
			GameObject go = Instantiate(CharacterFactory.GetPrefab(type));
			go.transform.SetParent(GameObject.Find("Environment/Map/Entities").transform, false);
			go.transform.position = Building.transform.position;
			var character = go.GetComponent<BaseCharacter>();
			character?.Setup(Building);
			return character;
		}
	}
}
