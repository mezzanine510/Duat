using UnityEngine;

public class SoulMover : MonoBehaviour
{
    public void Move(Vector3 target)
    {
        transform.position += target;
    }
}
