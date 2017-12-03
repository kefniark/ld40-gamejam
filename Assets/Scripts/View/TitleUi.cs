using System;

using DG.Tweening;

using UnityEngine;

namespace Assets.Scripts.View
{
	public class TitleUi : MonoBehaviour
	{
		public Transform Explain1;
		public Transform Explain2;
		public CanvasGroup ExplainCanvas;

		private Action OnComplete;
		public CanvasGroup Title;

		public void ShowTitle(Action startGame)
		{
			OnComplete = startGame;
			Title.alpha = 0;
			ExplainCanvas.alpha = 0;

			gameObject.SetActive(true);
			Title.DOFade(1, 0.3f).OnStart(() => Title.gameObject.SetActive(true));
		}

		public void ShowExplain1()
		{
			Explain1.gameObject.SetActive(true);
			ExplainCanvas.DOFade(1, 0.3f).OnStart(() => ExplainCanvas.gameObject.SetActive(true));
		}

		public void ClickStart()
		{
			Title.DOFade(0, 0.2f).OnComplete(
				() => {
					Title.gameObject.SetActive(false);
					ShowExplain1();
				});
		}

		public void ClickExplainNext()
		{
			Explain1.gameObject.SetActive(false);
			Explain2.gameObject.SetActive(true);
		}

		public void ClickExplain2Next()
		{
			Explain2.gameObject.SetActive(false);
			ExplainCanvas.DOFade(0, 0.2f).OnComplete(
				() => {
					ExplainCanvas.gameObject.SetActive(false);
					gameObject.SetActive(false);

					OnComplete?.Invoke();
				});
		}
	}
}