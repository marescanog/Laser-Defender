using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Path Loop Config")]

public class PathLoop : ScriptableObject
{
    [SerializeField] GameObject pathLoopPrefab;
    [SerializeField] float moveSpeed = 2f;

    public List<Transform> GetPathPoints()
    {
        var pathWaypoints = new List<Transform>();
        foreach (Transform child in pathLoopPrefab.transform)
        {
            pathWaypoints.Add(child);
        }
        return pathWaypoints;
    }

    public float GetPathMoveSpeed() { return moveSpeed; }
}
