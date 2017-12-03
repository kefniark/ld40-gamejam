using Assets.Scripts.Components.StateMachine;
using Assets.Scripts.View;

using DG.Tweening;

using UnityEngine;

namespace Assets.Scripts.States
{
	public class StateTitle : State<GameStates, GameController>
	{
		public StateTitle(TitleUi title, GameUi ui)
		{
			ui.gameObject.SetActive(false);
			Entered += (sender, arg) => title.ShowTitle(() =>
			{
				StateMachine.ChangeState(GameStates.Game);
				ui.gameObject.SetActive(true);
				ui.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
			});
		}
	}
}
