using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NPCPathfinding : MonoBehaviour
{
    [Header("Настройки пути")]
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private bool _isLooping = true;
    [SerializeField] private float[] _waitTimes;

    [Header("Настройки анимации")]
    [SerializeField] private string _speedParameter = "Speed";
    [SerializeField] private float _animationSmoothing = 5f;

    [Header("Настройки диалога")]
    [SerializeField] private bool _stopForDialogue = true; // Останавливать ли NPC при диалоге
    [SerializeField] private bool _resumeAfterDialogue = true; // Продолжать ли движение после диалога

    private NavMeshAgent _agent;
    private Animator _animator;
    private int _currentPointIndex = 0;
    private float _waitTimer;
    private bool _isWaiting;
    private float _currentSpeed;

    // Новые поля для управления диалогом
    private bool _isTalking;
    private Vector3 _stopPosition;
    private bool _wasStoppedBeforeDialogue;
    private bool _wasWaitingBeforeDialogue;
    private float _remainingWaitTime; // Сохраняем оставшееся время ожидания

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        ValidateArrays();

        if (_waypoints.Length > 0)
        {
            GoToNextPoint();
        }
        else
        {
            Debug.LogWarning($"У {gameObject.name} не заданы точки патрулирования!");
        }
    }

    void Update()
    {
        if (_waypoints.Length == 0) return;

        // Если NPC в диалоге - не обрабатываем движение
        if (_isTalking) return;

        // Проверяем, дошел ли агент до точки
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (!_isWaiting)
            {
                StartWaiting();
            }
            else
            {
                HandleWaiting();
            }
        }

        // Обновляем анимации
        UpdateAnimations();
    }

    private void ValidateArrays()
    {
        if (_waypoints.Length > 0)
        {
            if (_waitTimes.Length != _waypoints.Length)
            {
                Debug.LogWarning($"Количество времен ожидания ({_waitTimes.Length}) не совпадает " +
                               $"с количеством точек ({_waypoints.Length}). Будет использовано значение по умолчанию: 1.5f");

                float[] defaultWaitTimes = new float[_waypoints.Length];
                for (int i = 0; i < _waypoints.Length; i++)
                {
                    defaultWaitTimes[i] = (_waitTimes.Length > i && _waitTimes[i] > 0) ? _waitTimes[i] : 1.5f;
                }
                _waitTimes = defaultWaitTimes;
            }
        }
    }

    private void GoToNextPoint()
    {
        _isWaiting = false;

        if (_currentPointIndex >= _waypoints.Length)
        {
            if (_isLooping)
                _currentPointIndex = 0;
            else
            {
                _agent.isStopped = true;
                return;
            }
        }

        if (!_agent.SetDestination(_waypoints[_currentPointIndex].position))
        {
            Debug.LogWarning($"Не могу построить путь к точке {_currentPointIndex}! Пропускаем...");
            _currentPointIndex++;
            GoToNextPoint();
            return;
        }

        _agent.isStopped = false;

        int nextIndex = _currentPointIndex + 1;

        if (nextIndex >= _waypoints.Length)
        {
            if (_isLooping)
                _currentPointIndex = 0;
            else
                _currentPointIndex = nextIndex;
        }
        else
        {
            _currentPointIndex = nextIndex;
        }
    }

    private void StartWaiting()
    {
        _isWaiting = true;

        int currentWaitIndex = GetCurrentWaitIndex();

        if (currentWaitIndex < _waitTimes.Length)
            _waitTimer = _waitTimes[currentWaitIndex];
        else
            _waitTimer = 1.5f;

        _agent.isStopped = true;
    }

    private int GetCurrentWaitIndex()
    {
        if (_currentPointIndex == 0 && !_isLooping && _waypoints.Length > 0)
        {
            return _waypoints.Length - 1;
        }
        else if (_currentPointIndex == 0 && _isLooping)
        {
            return _waypoints.Length - 1;
        }
        else
        {
            int index = _currentPointIndex - 1;
            return index < 0 ? 0 : index;
        }
    }

    private void HandleWaiting()
    {
        _waitTimer -= Time.deltaTime;
        if (_waitTimer <= 0f)
        {
            _agent.isStopped = false;
            GoToNextPoint();
        }
    }

    private void UpdateAnimations()
    {
        if (_animator == null) return;

        float targetSpeed = _agent.velocity.magnitude;

        if (_agent.isStopped || _isWaiting || _isTalking)
            targetSpeed = 0f;

        _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, Time.deltaTime * _animationSmoothing);
        _animator.SetFloat(_speedParameter, _currentSpeed);
    }

    // ========== НОВЫЕ МЕТОДЫ ДЛЯ ДИАЛОГА ==========

    /// <summary>
    /// Вызывается из DialogueTrigger или DialogueManager когда начинается диалог
    /// </summary>
    public void StartDialogue()
    {
        if (!_stopForDialogue) return;

        _isTalking = true;

        // Сохраняем текущее состояние
        _wasStoppedBeforeDialogue = _agent.isStopped;
        _wasWaitingBeforeDialogue = _isWaiting;

        // Сохраняем оставшееся время ожидания, если NPC ждал
        if (_isWaiting)
        {
            _remainingWaitTime = _waitTimer;
        }

        // Запоминаем текущую позицию
        _stopPosition = transform.position;

        // Останавливаем агента
        _agent.isStopped = true;
        _agent.ResetPath(); // Сбрасываем текущий путь

        // Останавливаем ожидание
        _isWaiting = false;

        Debug.Log($"{gameObject.name} остановлен для диалога");
    }

    /// <summary>
    /// Вызывается из DialogueManager когда диалог заканчивается
    /// </summary>
    public void EndDialogue()
    {
        if (!_stopForDialogue) return;

        _isTalking = false;

        if (!_resumeAfterDialogue)
        {
            Debug.Log($"{gameObject.name} не продолжает движение после диалога");
            return;
        }

        // Возвращаемся к патрулированию
        if (_wasWaitingBeforeDialogue)
        {
            // Если до диалога NPC ждал - продолжаем ожидание
            _isWaiting = true;
            _waitTimer = _remainingWaitTime;
            _agent.isStopped = true;
            Debug.Log($"{gameObject.name} продолжает ожидание ({_waitTimer} сек.)");
        }
        else
        {
            // Если NPC двигался - продолжаем движение к текущей точке
            ResumePathfinding();
        }

        Debug.Log($"{gameObject.name} продолжает движение после диалога");
    }

    /// <summary>
    /// Возобновляет движение к текущей точке
    /// </summary>
    private void ResumePathfinding()
    {
        if (_currentPointIndex >= _waypoints.Length)
        {
            if (_isLooping)
                _currentPointIndex = 0;
            else
                return;
        }

        // Получаем индекс точки, к которой шли
        int targetPointIndex;
        if (_currentPointIndex == 0 && _isLooping)
            targetPointIndex = 0;
        else if (_currentPointIndex > 0)
            targetPointIndex = _currentPointIndex - 1;
        else
            targetPointIndex = 0;

        // Устанавливаем цель
        if (targetPointIndex >= 0 && targetPointIndex < _waypoints.Length)
        {
            if (!_agent.SetDestination(_waypoints[targetPointIndex].position))
            {
                Debug.LogWarning("Не могу вернуться к маршруту после диалога");
                // Пропускаем текущую точку и идем к следующей
                GoToNextPoint();
            }
            else
            {
                _agent.isStopped = false;
            }
        }
    }

    /// <summary>
    /// Полная остановка NPC (для экстренных ситуаций)
    /// </summary>
    public void StopMoving()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
        _isWaiting = false;
        _isTalking = false;
    }

    /// <summary>
    /// Полное возобновление патрулирования
    /// </summary>
    public void ResumePatrol()
    {
        _isTalking = false;
        _isWaiting = false;
        _agent.isStopped = false;

        // Сбрасываем индекс и начинаем сначала
        _currentPointIndex = 0;
        GoToNextPoint();
    }

    /// <summary>
    /// Проверка, говорит ли NPC
    /// </summary>
    public bool IsTalking()
    {
        return _isTalking;
    }

    /// <summary>
    /// Получить текущую позицию для камеры диалога
    /// </summary>
    public Vector3 GetDialoguePosition()
    {
        return _stopPosition;
    }
}