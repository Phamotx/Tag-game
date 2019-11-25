using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Controller : MonoBehaviour
{
    [SerializeField]
    private LayerMask collisionLayer;
    const float widthOfSkin = 0.020f;
    [SerializeField]
    private int horizontalRayCount = 4;
    [SerializeField]
    private int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D collider;
    RaycastOriginPosition rayPos;

    public CollisonInfo collisonInfo;
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    private void Update()
    { 
       
    }

    public void Move(Vector3 velocity)
    {
        collisonInfo.Reset();
        UpdateRaycastOriginPosition();
        if (velocity.x != 0)
        {
            HorizontalCollison(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollison(ref velocity);
        }
        transform.Translate(velocity);
    }

    void HorizontalCollison(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + widthOfSkin;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? rayPos.bottomLeft : rayPos.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionLayer);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - widthOfSkin) * directionX;
                rayLength = hit.distance;
                collisonInfo.left = (directionX == -1);
                collisonInfo.right = (directionX == 1);
            }
        }
    }

    void VerticalCollison(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + widthOfSkin;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? rayPos.bottomLeft : rayPos.topLeft;
            rayOrigin += Vector2.right*(verticalRaySpacing*i+velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionLayer);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY*rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance-widthOfSkin) * directionY;
                rayLength = hit.distance;
                collisonInfo.below = (directionY == -1);
                collisonInfo.above = (directionY == 1);
            }
        }
    }

    void UpdateRaycastOriginPosition()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(widthOfSkin * -2f);

        rayPos.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        rayPos.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        rayPos.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        rayPos.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(widthOfSkin * -2f);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
    struct RaycastOriginPosition
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
    public struct CollisonInfo
    {
        public bool above, below;
        public bool right, left;
        public void Reset()
        {
            above = below = right = left = false;
        }
    }
}
