using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outlineScript : MonoBehaviour
{
    [SerializeField] private bool triggered;
    [SerializeField] private Sprite withoutOutline;
    [SerializeField] private Sprite withOutline;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) spriteRenderer.sprite = withOutline;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) spriteRenderer.sprite = withoutOutline;
    }
}
