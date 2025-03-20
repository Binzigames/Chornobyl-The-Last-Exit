using UnityEngine;

public class AnomalyPushback : MonoBehaviour
{
    public float pushForce = 10f; // Сила відштовхування

    private void OnTriggerStay(Collider other)
    {
        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller != null)
        {
            Vector3 pushDirection = (other.transform.position - transform.position).normalized;
            controller.Move(pushDirection * pushForce * Time.deltaTime);
        }
    }
}
