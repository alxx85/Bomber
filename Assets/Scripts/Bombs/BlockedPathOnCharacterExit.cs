using UnityEngine;

public class BlockedPathOnCharacterExit : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Characters character))
        {
            GetComponent<Collider>().isTrigger = false;
        }
    }
}
