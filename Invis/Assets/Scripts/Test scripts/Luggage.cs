using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Luggage : MonoBehaviour
{

    public TextMeshProUGUI Hint;
    public TextMeshProUGUI HintLuggage;
    public string HintText = "";
    public KeyCode LookForLuggage = KeyCode.E;
    private bool isPlayerInside = false;


    private void OnTriggerEnter(Collider other)
    {
        Hint.text = HintText;
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (Hint != null) Hint.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        // Проверяем нажатие кнопки каждое мгновение, но только если игрок внутри
        if (isPlayerInside && Input.GetKeyDown(LookForLuggage))
        {
            LookingForLuggage();
        }
    }

    private void LookingForLuggage()
    {
        Hint.gameObject.SetActive(false);
        HintLuggage.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (Hint != null)
            {
                Hint.gameObject.SetActive(false);
                HintLuggage.gameObject.SetActive(false);
            }
        }
    }

}
