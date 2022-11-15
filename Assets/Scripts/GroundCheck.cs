using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private bool isGrounded;

    public bool IsGrounded()
    {
        return isGrounded;
    }

    private void OnTriggerEnter(Collider collision)
    {
        isGrounded = true;
    }

    private void OnTriggerExit(Collider collision)
    {
        isGrounded = false;
    }
}

