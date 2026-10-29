using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private AudioClip springNoise;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(25f, 19f), ForceMode2D.Impulse);
        AudioManager.instance.PlaySound(springNoise);
    }
}
