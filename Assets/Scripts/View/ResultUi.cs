using DG.Tweening;

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
	public class ResultUi : MonoBehaviour
	{
		private bool anticlick;
		public CanvasGroup Canvas;
		public Text Text;
		public AudioMixer AudioSfx;

		public void Awake()
		{
			AudioSfx.DOSetFloat("SfxVolume", -50, 3f).SetDelay(3);
		}

		public void ShowResult(float score, float duration)
		{
			Canvas.alpha = 0;
			anticlick = true;
			Text.text = Text.text.Replace("{0}", $"{duration:0.00}");
			Text.text = Text.text.Replace("{1}", $"{score:000000}");
			gameObject.SetActive(true);
			Canvas.DOFade(1, 0.3f).OnStart(() => Canvas.gameObject.SetActive(true));
		}

		public void ClickRetry()
		{
			if (!anticlick)
			{
				return;
			}
			anticlick = false;
			SceneManager.LoadScene("Game");
		}
	}
}
