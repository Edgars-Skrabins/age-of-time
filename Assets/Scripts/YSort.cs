using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSort : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int sortingOffset = 0;
    [SerializeField] private float precisionMultiplier = 100f; // Higher = better precision

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        spriteRenderer.sortingOrder = -(int)(transform.position.y * precisionMultiplier) + sortingOffset;
    }
}
