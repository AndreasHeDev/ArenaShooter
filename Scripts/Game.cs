using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
  public static Game Instance;
  private const int FieldSize = 45;
  private Player player;
  private Vector3 playerSpawnPosition;
  private List<Character> enemies = new List<Character>();
  private int difficulty = 1;
  private int difficultyRaiseThreshhold = 4;
  private int highscore;
  public event Action<int> OnHighscoreChange;
  [SerializeField]
  private Enemy enemyPrefabs;
  [SerializeField]
  private float spawnDistance = 5f;

  void Awake()
  {
    if (Instance == null)
      Instance = this;
    else
      Debug.Log("There are more than 1 Instance of Player");
  }

  void Start()
  {
    player = Player.Instance;
    playerSpawnPosition = player.transform.position;
    player.OnCharacterDie += OnPlayerDie;
  }

  void OnDestroy() => player.OnCharacterDie -= OnPlayerDie;

  private void OnPlayerDie(Character player)
  {
    StopAllCoroutines();
    foreach (var enemy in enemies)
      enemy.enabled = false;
  }

  private IEnumerator SpawnEnemies()
  {
    while (enabled)
    {
      yield return new WaitForSeconds(3f / difficulty);
      SpawnEnemie();
    }
  }

  private void SpawnEnemie()
  {
    var randomEnemyPosition = GetRandomPositionAroundPlayer();
    var enemy = Instantiate(enemyPrefabs, randomEnemyPosition, Quaternion.identity);
    enemies.Add(enemy);
    enemy.OnCharacterDie += OnEnemyDie;
  }

  private Vector3 GetRandomPositionAroundPlayer()
  {
    var angle = UnityEngine.Random.Range(0, 360);
    var posX = IncludeOffset(player.transform.position.x, Mathf.Cos(angle));
    var posZ = IncludeOffset(player.transform.position.z, Mathf.Sin(angle));
    return new Vector3(posX, 0, posZ);
  }

  private float IncludeOffset(float charPos, float anglePos)
  {
    var rangeMultiplicator = spawnDistance + difficulty;
    var pos = charPos + anglePos * rangeMultiplicator;
    if (pos < -FieldSize)
      return SetOffsetDifferenz(pos, pos + FieldSize);
    if (pos > FieldSize)
      return SetOffsetDifferenz(pos, pos - FieldSize);
    return pos;
  }

  private float SetOffsetDifferenz(float pos, float differenz) => pos - differenz * 2;

  private void OnEnemyDie(Character enemy)
  {
    enemies.Remove(enemy);
    enemy.OnCharacterDie -= OnEnemyDie;
    UpdateHighscore(highscore + 1);
    UpdateDifficulty();
  }

  private void UpdateHighscore(int newHighscore)
  {
    highscore = newHighscore;
    OnHighscoreChange?.Invoke(highscore);
  }

  private void UpdateDifficulty()
  {
    if (highscore >= difficultyRaiseThreshhold)
    {
      difficultyRaiseThreshhold *= 2;
      difficulty++;
    }
  }

  public void StartNewGame()
  {
    player.transform.position = playerSpawnPosition;
    player.GetComponent<Health>().Revive();
    difficulty = 1;
    difficultyRaiseThreshhold = 4;
    UpdateHighscore(0);
    RemoveAllSpawnedAmmunition();
    RemoveAllEnemies();
    StartCoroutine(SpawnEnemies());
  }

  private void RemoveAllSpawnedAmmunition()
  {
    foreach (var spawnedItem in Ammunition.SpawnedAmmunition)
      Destroy(spawnedItem.gameObject);
    Ammunition.SpawnedAmmunition.Clear();
  }

  private void RemoveAllEnemies()
  {
    foreach (var enemy in enemies)
    {
      enemy.OnCharacterDie -= OnEnemyDie;
      Destroy(enemy.gameObject);
    }
    enemies.Clear();
  }
}
