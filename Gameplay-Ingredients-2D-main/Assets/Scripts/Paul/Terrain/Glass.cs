using UnityEngine;

public class Glass : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Weight"))
        {
            Destroy(collision.gameObject);
            gameObject.SetActive(false);
        }
    }
}
