using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemSpawner : MonoBehaviour
{
    public GameObject Object;
    public TextMeshProUGUI Hint;
    public string HintText = "";
    public KeyCode SpawnItem = KeyCode.E;
    [SerializeField] private Collider _triggerCollider;


    private bool isPlayerInside = false;

    private void Spawn()
    {
        Object.SetActive(true);
        Hint.gameObject.SetActive(false);
        _triggerCollider.enabled = false;    
    }
   
    void Start()
    {
        if (Hint != null) Hint.gameObject.SetActive(false);
        if (Object != null) Object.gameObject.SetActive(false);
        Hint.text = HintText;
    }

    
    void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(SpawnItem))
        {
            Spawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Hint.text = HintText;
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (Hint != null) Hint.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (Hint != null) Hint.gameObject.SetActive(false);
        }
    }
}
