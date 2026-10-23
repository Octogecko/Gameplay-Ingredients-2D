using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (gameObject.name)
        {
            case "Spikes":
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<PlayerLife>().Hurt(damage);
                }
                break;
            case "Dash Portal":
                if (collision.gameObject.CompareTag("Player") && !collision.gameObject.GetComponent<PlayerMovement>().isDashing)
                {
                    GetComponent<BoxCollider2D>().isTrigger = false;
                    collision.gameObject.GetComponent<PlayerLife>().Hurt(damage);
                    collision.gameObject.GetComponent<Rigidbody2D>().linearVelocityX = collision.gameObject.GetComponent<Rigidbody2D>().linearVelocityX*1000;
                    Invoke("MakeTrigger", 0.5f);
                }
                break;
        }
    }

    private void MakeTrigger()
    {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
