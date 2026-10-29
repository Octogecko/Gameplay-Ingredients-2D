using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spaceship : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Sprite playerInSpaceship;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject winCanvas;
    private bool inZone = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inZone = true;
        InteractionUI.instance.ShowInteraction("Press E to enter the spaceship");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inZone = false;
        InteractionUI.instance.HideInteraction();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && inZone)
        {
            player.SetActive(false);
            Instantiate(explosion, transform.position, new Quaternion());
            InventoryUI.Instance.enabled = false;
            Invoke("ActivateWinCanvas", 1f);
            gameObject.SetActive(false);
        }
    }

    private void ActivateWinCanvas()
    {
        winCanvas.SetActive(true);
    }
}
