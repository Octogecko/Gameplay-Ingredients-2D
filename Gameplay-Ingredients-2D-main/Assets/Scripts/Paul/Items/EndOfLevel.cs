using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerInventory.Instance.IsInInventory("REDGEM"))
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                InteractionUI.instance.ShowInteraction("You are lacking something");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InteractionUI.instance.HideInteraction();
        }
    }
}
