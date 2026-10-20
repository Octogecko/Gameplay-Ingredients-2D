using UnityEngine;

public class Lever : MonoBehaviour
{
    private bool inZone = false;
    private bool repaired = false;
    private bool activated = false;

    [SerializeField] private Sprite leverDeactivated;
    [SerializeField] private Sprite leverActivated;
    [SerializeField] private Animator bridgeAnimator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inZone && PlayerInventory.Instance.IsInInventory("LEVERHANDLE") && !repaired)
        {
            repaired = true;
            GetComponent<SpriteRenderer>().sprite = leverDeactivated;
            InteractionUI.instance.ShowInteraction("Press E to activate the lever");
            PlayerInventory.Instance.RemoveItemFromInventory("LEVERHANDLE");
        }
        else if (Input.GetKeyDown(KeyCode.E) && inZone && repaired && !activated)
        {
            activated = true;
            GetComponent<SpriteRenderer>().sprite = leverActivated;
            bridgeAnimator.SetTrigger("Down");
            InteractionUI.instance.HideInteraction();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (PlayerInventory.Instance.IsInInventory("LEVERHANDLE") || repaired && !activated))
        {
            inZone = true;
            InteractionUI.instance.ShowInteraction((repaired ? "Press E to activate the lever" : "Press E to repair the lever"));
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
}
