using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Renderer))]
public class InfiniteParallax : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform targetTransform;

    [Header("Parallax Settings")]
    [SerializeField] private float parallaxStep = 0.1f;
    [SerializeField] private bool affectVertical = false;

    [Header("Infinite Loop Settings")]
    [SerializeField] private bool infiniteHorizontal = true;

    private Transform cameraTransform;
    private Vector3 lastTargetPosition;
    private float parallaxMultiplier;
    private float textureUnitSizeX;

    private void Start()
    {
        cameraTransform = Camera.main.transform;

        if (targetTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                targetTransform = player.transform;
            }
            else
            {
                PlayerController pc = FindFirstObjectByType<PlayerController>();
                if (pc != null) targetTransform = pc.transform;
            }
        }

        if (targetTransform != null)
        {
            lastTargetPosition = targetTransform.position;
        }

        Renderer rend = GetComponent<Renderer>();
        parallaxMultiplier = rend.sortingOrder * parallaxStep;

        if (TryGetComponent<Tilemap>(out Tilemap tilemap))
        {
            tilemap.CompressBounds();
            textureUnitSizeX = tilemap.localBounds.size.x * transform.lossyScale.x;
        }
        else if (rend is SpriteRenderer spriteRenderer && spriteRenderer.sprite != null)
        {
            if (spriteRenderer.drawMode != SpriteDrawMode.Simple)
            {
                textureUnitSizeX = spriteRenderer.size.x * transform.lossyScale.x;
            }
            else
            {
                textureUnitSizeX = spriteRenderer.bounds.size.x;
            }
        }
        else
        {
            textureUnitSizeX = rend.bounds.size.x;
        }
    }

    private void LateUpdate()
    {
        if (targetTransform == null) return;

        Vector3 deltaMovement = targetTransform.position - lastTargetPosition;
        float moveX = deltaMovement.x * parallaxMultiplier;
        float moveY = affectVertical ? deltaMovement.y * parallaxMultiplier : 0f;

        transform.position += new Vector3(moveX, moveY, 0f);
        lastTargetPosition = targetTransform.position;

        if (infiniteHorizontal && textureUnitSizeX > 0f && cameraTransform != null)
        {
            float relativeDistanceX = cameraTransform.position.x - transform.position.x;

            if (Mathf.Abs(relativeDistanceX) >= textureUnitSizeX)
            {
                float offsetPositionX = relativeDistanceX % textureUnitSizeX;
                transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y, transform.position.z);
            }
        }
    }
}