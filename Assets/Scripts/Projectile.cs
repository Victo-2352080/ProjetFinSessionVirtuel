
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float vitesse = 20f;
    [SerializeField] private float distanceMax = 5f;

    private Vector3 positionDepart;

    void Start()
    {
        positionDepart = transform.position;
    }

    void Update()
    {
        float distanceFrame = vitesse * Time.deltaTime;

        // Raycast pour �viter de traverser les objets (important � haute vitesse)
        if (Physics.Raycast(transform.position, -transform.right, out RaycastHit hit, distanceFrame))
        {
            IHittable hittable = hit.collider.GetComponent<IHittable>();

            if (hittable != null)
            {
                hittable.OnHit();
            }

            Destroy(gameObject);
            return;
        }

        // Avancer
        transform.Translate(Vector3.left * distanceFrame);

        // V�rifier distance max
        if (Vector3.Distance(positionDepart, transform.position) >= distanceMax)
        {
            Destroy(gameObject);
        }
    }
}