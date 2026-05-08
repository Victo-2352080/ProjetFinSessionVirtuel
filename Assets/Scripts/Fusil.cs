using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Fusil : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform pointDeTir;

    [Header("Reset Position")]
    [SerializeField] private Transform restPosition;

    private XRGrabInteractable grab;

    private SRGameManager GMInstance;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        GMInstance = SRGameManager.Instance;
    }

    void OnEnable()
    {
        grab.activated.AddListener(OnTriggerPressed);
        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    void OnDisable()
    {
        grab.activated.RemoveListener(OnTriggerPressed);
        grab.selectEntered.RemoveListener(OnGrabbed);
        grab.selectExited.RemoveListener(OnReleased);
    }

    void Update()
    {
        if (!GMInstance.IsGameStarted) ResetGun();
    }

    private void ResetGun()
    {
        if (restPosition == null) return;

        // reposition
        transform.position = restPosition.position;
        transform.rotation = restPosition.rotation;
    }

    private void OnTriggerPressed(ActivateEventArgs args)
    {
        if (!grab.isSelected || !GMInstance.IsGameStarted) return;

        Tirer();
    }

    private void Tirer()
    {
        Instantiate(projectilePrefab, pointDeTir.position, pointDeTir.rotation);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (!GMInstance.IsGameStarted)
        {
            GMInstance.StartGame();
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        // Effectuer une action si on lâche le gun
    }
}