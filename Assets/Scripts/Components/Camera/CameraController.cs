using DG.Tweening;

using UnityEngine;

namespace Assets.Scripts.Components
{
	public class CameraController : MonoBehaviour
	{
		public Vector2 HorizontalBounds = new Vector2(-30, 30);
		public Vector2 VerticalBounds = new Vector2(-32, 32);

		public bool IsEnabled = true;

		private bool moveDown;
		private bool moveLeft;
		private bool moveRight;
		private bool moveUp;

		public float Speed = 10f;
		public Transform Target;

		public void Awake()
		{
			Application.targetFrameRate = 60;
		}

		private void Update()
		{
			if (!IsEnabled)
			{
				return;
			}

			if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
			{
				moveDown = true;
			}
			else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
			{
				moveDown = false;
			}

			if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
			{
				moveUp = true;
			}
			else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
			{
				moveUp = false;
			}

			if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
			{
				moveRight = true;
			}
			else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
			{
				moveRight = false;
			}

			if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
			{
				moveLeft = true;
			}
			else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
			{
				moveLeft = false;
			}

			Move();
		}

		/// <summary>
		/// Move the target Camera
		/// </summary>
		private void Move()
		{
			Vector3 position = Target.transform.position;

			if (moveDown)
			{
				position += Vector3.forward * Time.deltaTime * Speed;
			}
			else if (moveUp)
			{
				position += Vector3.back * Time.deltaTime * Speed;
			}

			if (moveLeft)
			{
				position += Vector3.right * Time.deltaTime * Speed;
			}
			else if (moveRight)
			{
				position += Vector3.left * Time.deltaTime * Speed;
			}

			// horizontal bounds
			position.x = Mathf.Max(position.x, HorizontalBounds.x);
			position.x = Mathf.Min(position.x, HorizontalBounds.y);

			// vertical bounds
			position.z = Mathf.Max(position.z, VerticalBounds.x);
			position.z = Mathf.Min(position.z, VerticalBounds.y);

			// move camera target
			Target.transform.position = position;
		}

		/// <summary>
		/// Use dotween to move the camera to the position we want to show to the user
		/// </summary>
		/// <param name="transformPosition"></param>
		public void MoveTo(Vector3 transformPosition)
		{
			moveLeft = false;
			moveRight = false;
			moveUp = false;
			moveDown = false;

			Target.DOMove(new Vector3(transformPosition.x, 0, transformPosition.z), 0.3f)
				.OnStart(() => IsEnabled = false)
				.OnComplete(() => IsEnabled = true);
		}
	}
}
