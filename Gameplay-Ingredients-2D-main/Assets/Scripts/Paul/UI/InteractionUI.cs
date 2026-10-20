using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI instance;
    [SerializeField] private TextMeshProUGUI interactionText;
    private void Awake()
    {
        instance = this;    
    }

    public void ShowInteraction(string text)
    {
        interactionText.text = text;
        interactionText.gameObject.SetActive(true);
    }

    public void HideInteraction()
    {
        interactionText.gameObject.SetActive(false);
    }
}
