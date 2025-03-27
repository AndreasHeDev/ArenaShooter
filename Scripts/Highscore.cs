using UnityEngine;
using TMPro;

public class Highscore : MonoBehaviour
{
  private TextMeshProUGUI text;

  void Start()
  {
    text = GetComponent<TextMeshProUGUI>();
    Game.Instance.OnHighscoreChange += OnHighscoreChange;
  }

  void OnDestroy() => Game.Instance.OnHighscoreChange -= OnHighscoreChange;
  private void OnHighscoreChange(int highscore) => text.text = $"Kills: {highscore}";
}
