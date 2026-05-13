using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.CompilerServices;

public class Note : MonoBehaviour

{
    public GameObject Note_UI;
    public TextMeshProUGUI Hintt;
    public string HintText = "";
    public KeyCode To_read = KeyCode.E;
    private bool isPlayerInside = false;

    private void Read()
    {
        Note_UI.SetActive(true);
        Hintt.gameObject.SetActive(false);
    }

    void Start()
    {
        if (Hintt != null) Hintt.gameObject.SetActive(false);
        Hintt.text = HintText;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(To_read))
        {
            Read();
            Cursor.lockState = CursorLockMode.None; // Разблокируем курсор
            Cursor.visible = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Hintt.text = HintText;
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (Hintt != null) Hintt.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (Hintt != null) Hintt.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked; // Заблокировать в центре
            Cursor.visible = false;
            Note_UI.SetActive(false);

        }
    }
}
