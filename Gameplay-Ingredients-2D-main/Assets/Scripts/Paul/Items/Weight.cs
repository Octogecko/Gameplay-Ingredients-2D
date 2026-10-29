using UnityEngine;

public class Weight : MonoBehaviour
{
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Void"))
        {
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            transform.position = originalPosition;
        }
    }
}
