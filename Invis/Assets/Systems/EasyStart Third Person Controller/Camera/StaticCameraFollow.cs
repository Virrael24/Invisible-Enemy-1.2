using UnityEngine;

public class StaticCameraFollow : MonoBehaviour
{
    [Header("Кого преследовать")]
    public Transform target;

    [Header("Расстояние от игрока")]
    public Vector3 offset = new Vector3(0, 10, -5); // Настройте высоту и отступ по вкусу

    void LateUpdate()
    {
        if (target != null)
        {
            // Камера просто копирует позицию игрока + заданный отступ
            // Но её Rotation остается неизменным (тем, что вы задали в инспекторе)
            transform.position = target.position + offset;
        }
    }
}