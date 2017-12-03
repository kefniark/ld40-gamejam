using Assets.Scripts.Components.StateMachine;
using Assets.Scripts.View;

using DG.Tweening;

using UnityEngine;

namespace Assets.Scripts.States
{
	public class StateResult : State<GameStates, GameController>
	{
		private readonly ResultUi Result;
		private readonly GameUi UI;
		public StateResult(ResultUi result, GameUi ui)
		{
			Result = result;
			UI = ui;
			Entered += StateResult_Entered;
		}

		private void StateResult_Entered(object sender, System.EventArgs e)
		{
			UI.GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() => UI.gameObject.SetActive(false));
			Result.ShowResult(Entity.Score, Entity.Duration);
		}
	}
}
