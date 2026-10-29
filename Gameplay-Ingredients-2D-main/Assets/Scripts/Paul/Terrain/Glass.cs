using UnityEngine;

public class Glass : MonoBehaviour
{
    [SerializeField] private AudioClip breakingGlass;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Weight"))
        {
            AudioManager.instance.PlaySound(breakingGlass);
            Destroy(collision.gameObject);
            gameObject.SetActive(false);
        }
    }   
}
