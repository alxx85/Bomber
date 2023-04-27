using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInput : Inputs
{
    [SerializeField] private LayerMask _blockedMask;
    private List<Vector3> _possibleDirections = new List<Vector3> { Vector3.left, Vector3.forward, Vector3.back, Vector3.right };
    private System.Random _getRandom = new System.Random();

    public override Vector3 GetDirection()
    {
        List<Vector3> blockedDirections = GetBlockedDirection();
        List<Vector3> directions = _possibleDirections.Except(blockedDirections).ToList();

        if (directions.Count > 0)
        {
            int directionIndex = _getRandom.Next(directions.Count());
            return directions[directionIndex];
        }

        return Vector3.zero;
    }

    public Vector3 GetApproximatePosition(Vector3 position)
    {
        float roundPositionX = position.x - GetRoundPosition(position).x;
        float roundPositionZ = position.z - GetRoundPosition(position).z;

        if (Mathf.Abs(roundPositionX) > 0.6f)
            roundPositionX -= 1;

        if (Mathf.Abs(roundPositionZ) > 0.6f)
            roundPositionZ -= 1;

        return new Vector3(roundPositionX, 0, roundPositionZ);
    }

    private List<Vector3> GetBlockedDirection()
    {
        List<Vector3> blockedDirection = new List<Vector3>();
        Vector3 position = GetRoundPosition(transform.position);
        position.y = .5f;

        Collider[] colliders = Physics.OverlapSphere(position, .8f, _blockedMask);

        foreach (var item in colliders)
        {
            Vector3 direction = item.transform.position - position;
            blockedDirection.Add(direction);
        }

        return blockedDirection;
    }
}