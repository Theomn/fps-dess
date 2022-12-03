using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public LayerMask layerMask;
    private bool isGrounded;

    public bool IsGrounded()
    {
        return isGrounded;
    }

    private void OnTriggerStay(Collider collision)
    {
        if ((layerMask & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if ((layerMask & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            isGrounded = false;
        }
    }
}

