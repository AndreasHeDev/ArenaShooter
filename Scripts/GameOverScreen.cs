using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
	private CanvasGroup canvasGroup;

	void Start()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		Player.Instance.OnCharacterDie += OnPlayerDie;
	}

	private void OnPlayerDie(Character player) => SetCanvasGroup(1);

	public void Restart()
	{
		SetCanvasGroup(0);
		Game.Instance.StartNewGame();
	}

	private void SetCanvasGroup(int alpha)
	{
		canvasGroup.alpha = alpha;
		canvasGroup.interactable = alpha == 1;
		canvasGroup.blocksRaycasts = alpha == 1;
	}
}