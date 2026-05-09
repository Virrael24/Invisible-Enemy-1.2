using UnityEngine;

public class CameraObstructionHandler : MonoBehaviour
{
    [Header("Настройки целей")]
    public Transform player;          // Перетащите сюда игрока
    public LayerMask obstructionMask; // Слой объектов, которые могут скрывать игрока (например, "Environment")

    [Header("Настройки материала")]
    public Material transparentMaterial; // Прозрачный или подсвеченный материал

    private GameObject _lastObstruction; // Объект, который мешал в прошлом кадре
    private Material _originalMaterial;  // Его исходный материал

    void Update()
    {
        if (player == null) return;

        // Определяем направление от камеры к игроку и дистанцию
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        // Пускаем луч
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, obstructionMask))
        {
            // Если луч попал во что-то, что НЕ является игроком
            if (hit.collider.gameObject != player.gameObject)
            {
                GameObject currentObstruction = hit.collider.gameObject;

                // Если это новый объект, который мы еще не обрабатывали
                if (currentObstruction != _lastObstruction)
                {
                    // Сначала восстанавливаем старый объект, если он был
                    RestoreOriginalMaterial();

                    // Сохраняем новый объект и его материал
                    _lastObstruction = currentObstruction;
                    MeshRenderer renderer = _lastObstruction.GetComponent<MeshRenderer>();

                    if (renderer != null)
                    {
                        _originalMaterial = renderer.material;
                        renderer.material = transparentMaterial;
                    }
                }
            }
            else
            {
                // Если луч попал сразу в игрока — путь свободен
                RestoreOriginalMaterial();
            }
        }
        else
        {
            // Если луч вообще ни во что не попал
            RestoreOriginalMaterial();
        }
    }

    private void RestoreOriginalMaterial()
    {
        if (_lastObstruction != null)
        {
            MeshRenderer renderer = _lastObstruction.GetComponent<MeshRenderer>();
            if (renderer != null && _originalMaterial != null)
            {
                renderer.material = _originalMaterial;
            }
            _lastObstruction = null;
            _originalMaterial = null;
        }
    }
}