using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private float spawnInterval = 2f;

    [Header("Target Config")]
    [SerializeField] private Vector3 movementDirection = Vector3.forward;
    [SerializeField] private float speed = 1f;
    [SerializeField] private int points = 10;
    [SerializeField] private float despawnDistance = 10f;

    private bool spawning = false;

    private float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void OnEnable()
    {
        SRGameManager.Instance.OnGameStart += CommencerSpawn;
        SRGameManager.Instance.OnGameEnd += ArreterSpawn;
    }

    void OnDisable()
    {
        SRGameManager.Instance.OnGameStart -= CommencerSpawn;
        SRGameManager.Instance.OnGameEnd -= ArreterSpawn;
    }

    public void CommencerSpawn()
    {
        StartCoroutine(StartSpawningWithDelay());
    }

    private IEnumerator StartSpawningWithDelay()
    {
        yield return new WaitForSeconds(4f);
        spawning = true;
    }

    public void ArreterSpawn()
    {
        spawning = false;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f && spawning)
        {
            Spawn();
            timer = spawnInterval;
        }
    }

    private void Spawn()
    {
        GameObject obj = Instantiate(targetPrefab, transform.position, transform.rotation);

        Targetable target = obj.GetComponent<Targetable>();

        if (target != null)
        {
            target.Init(movementDirection, speed, points, despawnDistance);
        }
    }
}