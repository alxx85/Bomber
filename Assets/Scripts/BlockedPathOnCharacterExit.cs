using UnityEngine;

public class BlockedPathOnCharacterExit : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            GetComponent<Collider>().isTrigger = false;
        }
    }
}
