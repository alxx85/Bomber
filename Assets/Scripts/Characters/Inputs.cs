using UnityEngine;

public abstract class Inputs : MonoBehaviour
{
    public abstract Vector3 GetDirection();

    public Vector3 GetRoundPosition(Vector3 position)
    {
        Vector3 currentPosition = position;
        currentPosition.x = Mathf.RoundToInt(currentPosition.x);
        currentPosition.y = 0f;
        currentPosition.z = Mathf.RoundToInt(currentPosition.z);
        return currentPosition;
    }
}
