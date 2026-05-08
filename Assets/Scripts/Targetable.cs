using UnityEngine;

public class Targetable : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private int points;
    private float despawnDistance;
    private bool active = true;

    private Vector3 startPosition;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
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
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}