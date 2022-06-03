using UnityEngine;

public class Mover : MonoBehaviour
{
    public void Move(Vector3 target)
    {
        transform.position += target;
    }
}