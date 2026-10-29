using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private GameObject targetTheDoor;
    [SerializeField] private bool theClosed;
    [SerializeField] private Sprite openTheDoorTop;
    [SerializeField] private Sprite openTheDoorBody;
    [SerializeField] private bool inZone = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inZone = true;
            InteractionUI.instance.ShowInteraction("Press E to interact");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inZone = false;
            InteractionUI.instance.HideInteraction();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && inZone)
        {
            if (theClosed && PlayerInventory.Instance.IsInInventory("YELLOWKEY"))
            {
                theClosed = false;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = openTheDoorBody;
                transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = openTheDoorTop;
                PlayerInventory.Instance.RemoveItemFromInventory("YELLOWKEY");
            }
            else if (theClosed)
            {
                InteractionUI.instance.ShowInteraction("You are missing something");
            }
            else
            {
                player.transform.position = targetTheDoor.transform.position;
            }
        }
    }
}
