using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTowardsMouseComponent : MonoBehaviour
{
    [SerializeField] Transform shootPosTransform;
    private Vector2 lookDirection;

    public Vector2 LookDirection { get => lookDirection; private set => lookDirection = value; }


    public Vector3 GetShootPosition() => shootPosTransform.position;
    public Vector2 GetShootDirection() => this.lookDirection;
    public Quaternion GetShootRotation() => shootPosTransform.rotation;

    /// <summary>
    /// Transforms an object to face the mouse cursor position in world space
    /// used for the player to be able to shoot at the mouse
    /// </summary>
    public void UpdateAimTowardsMouse()
    {
        Vector2 currentPosition = new Vector2(shootPosTransform.position.x, shootPosTransform.position.y);

        // Gets the world position of the mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = (mousePosition - currentPosition).normalized;
        
        // calculates the Vector2 direction between the shootPosTransform and the mouse
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        shootPosTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public Vector2 GetMousePosition()
    {
        Vector2 currentPosition = new Vector2(shootPosTransform.position.x, shootPosTransform.position.y);

        // Gets the world position of the mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, (mousePosition - currentPosition), Mathf.Abs(Vector2.Distance(mousePosition, currentPosition)), LayerMask.GetMask("Obstacle"));

        if (wallCheck) return wallCheck.point - (mousePosition - currentPosition).normalized;
        
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}