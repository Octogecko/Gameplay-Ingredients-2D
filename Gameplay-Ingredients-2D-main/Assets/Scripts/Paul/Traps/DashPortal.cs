using UnityEngine;

public class DashPortal : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject explosionInstance;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.gameObject.GetComponent<PlayerMovement>().isDashing)
        {
            collision.gameObject.GetComponent<PlayerLife>().Hurt(damage);
            collision.gameObject.GetComponent<PlayerMovement>().enabled = false;
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            explosionInstance = Instantiate(explosion, transform);
            collision.gameObject.GetComponent<PlayerMovement>().InvokeRespawn(0.5f);
            Invoke("DestroyExplosion", 0.5f);
        }
    }

    private void DestroyExplosion()
    {
        if (explosionInstance)
        {
            Destroy(explosionInstance);
        }
    }
}
