using UnityEngine;

public class PlaceableObj : MonoBehaviour
{
    private Vector2 _dir = Vector2.zero;
    public Vector2 GetDirection() => _dir;

    public void SetDirection(Vector2 dir)
    {
        _dir = dir;
    }
}
