using UnityEngine;

public class Targetable : MonoBehaviour, IHittable
{
    private Vector3 direction;
    private float speed;
    private int points;
    private float despawnDistance;
    private bool active = true;

    private Vector3 startPosition;

    private Animator animator;
    private AudioSource audioSource;

    [Header("Sound")]
    [SerializeField] private AudioClip hitSound;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Init(Vector3 movementDirection, float newSpeed, int newPoints, float newDespawnDistance)
    {
        direction = movementDirection.normalized;
        speed = newSpeed;
        points = newPoints;
        despawnDistance = newDespawnDistance;

        startPosition = transform.position;
    }

    void Update()
    {
        // Stop moving and destroy if game has ended
        if (!SRGameManager.Instance.IsGameStarted)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += direction * speed * Time.deltaTime;

        if ((transform.position - startPosition).sqrMagnitude >= despawnDistance * despawnDistance)
        {
            Destroy(gameObject);
        }
    }

    public int GetPoints()
    {
        return points;
    }

    public void OnHit()
    {
        if (!active) return;
        Debug.Log($"Hit +{points}");
        SRGameManager.Instance.AddScore(points);
        active = false;
        animator.SetTrigger("Hit");
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}