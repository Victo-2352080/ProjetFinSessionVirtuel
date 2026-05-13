using UnityEngine;

public class MenuButton : MonoBehaviour, IHittable
{
    public enum ButtonType { Play, Quit }
    [SerializeField] private ButtonType buttonType;

    void Start()
    {
        SRGameManager.Instance.OnGameStart += Hide;
        SRGameManager.Instance.OnGameEnd += Show;
    }

    void OnDestroy()
    {
        SRGameManager.Instance.OnGameStart -= Hide;
        SRGameManager.Instance.OnGameEnd -= Show;
    }

    private void Hide() => gameObject.SetActive(false);
    private void Show() => gameObject.SetActive(true);

    public void OnHit()
    {
        Debug.Log($"Button {buttonType} hit");
        HapticFeedback.SendHapticImpulse(0.5f, 0.12f);
        switch (buttonType)
        {
            case ButtonType.Play:
                SRGameManager.Instance.StartGame();
                break;
            case ButtonType.Quit:
                Application.Quit();
                break;
        }
    }
}