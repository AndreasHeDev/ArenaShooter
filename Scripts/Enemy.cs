using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : Character
{
  private const float timeTillDealingDamageAgain = 0.5f;
  private Player player;
  private NavMeshAgent agent;
  private AmmunitionSpawner ammunitionSpawner;
  private bool isTouchingPlayer;
  private bool coroutineIsRunning;

  void Start()
  {
    player = Player.Instance;
    agent = GetComponent<NavMeshAgent>();
    ammunitionSpawner = GetComponent<AmmunitionSpawner>();
    agent.SetDestination(player.transform.position);
    StartCoroutine(UpdateAgentPath());
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.tag.Equals("Player"))
    {
      isTouchingPlayer = true;
      if (!coroutineIsRunning)
        StartCoroutine(DamageTimer(other.GetComponent<Health>()));
    }
  }

  void OnTriggerExit(Collider other)
  {
    if (other.tag.Equals("Player"))
      isTouchingPlayer = false;
  }

  void OnDisable() => agent.isStopped = true;

  public override void Die()
  {
    base.Die();
    ammunitionSpawner.SpawnRandomAmmunition();
    Destroy(gameObject);
  }

  private IEnumerator UpdateAgentPath()
  {
    while (enabled)
    {
      yield return new WaitForSeconds(0.5f);
      agent.SetDestination(player.transform.position);
    }
  }

  private IEnumerator DamageTimer(Health player)
  {
    coroutineIsRunning = true;
    while (isTouchingPlayer)
    {
      player.TakeDamage(1);
      yield return new WaitForSeconds(timeTillDealingDamageAgain);
    }
    coroutineIsRunning = false;
  }
}
