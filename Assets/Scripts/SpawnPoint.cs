using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private float spawnInterval = 2f;

    [Header("Target Config")]
    [SerializeField] private Vector3 movementDirection = Vector3.forward;
    [SerializeField] private float speed = 1f;
    [SerializeField] private int points = 10;
    [SerializeField] private float despawnDistance = 10f;

    private float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
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