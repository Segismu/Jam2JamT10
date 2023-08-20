using UnityEngine;

[CreateAssetMenu(fileName = "CatConfig", menuName = "ScriptableObjects/CatConfig", order = 1)]

public class CatConfig : ScriptableObject

{
    public float initialJumpForce = 2f;
    public float climbSpeed = 2f;
    public float moveSpeed = 3f;
    public float maxJumpForce = 10f;
    public float jumpChargeRate = 2f;
    public Vector2 leftWallJumpForce = new(5, 4f);
    public Vector2 rightWallJumpForce = new(-5, 4f);
}