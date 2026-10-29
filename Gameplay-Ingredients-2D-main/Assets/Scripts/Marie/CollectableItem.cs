using System;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private Item itemData;
    [SerializeField] private AudioClip collect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInventory.Instance.AddItemToInventory(itemData);
            AudioManager.instance.PlaySound(collect);
            Destroy(gameObject);
        }
    }
}

[Serializable]
public class Item
{
    public string uniqueID;
    public Sprite icon;
}
