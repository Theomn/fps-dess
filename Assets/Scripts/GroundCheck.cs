using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private bool isGrounded;

    public bool IsGrounded()
    {
        return isGrounded;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
    }
}

