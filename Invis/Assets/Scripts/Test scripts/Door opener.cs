using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dooropener : MonoBehaviour
{
    public GameObject DoorOne;
    public GameObject DoorTwo;
    public TextMeshProUGUI Hint;
    public KeyCode RemoveDoors = KeyCode.E;

    private void OnTriggerEnter(Collider other)
    {
        Hint.gameObject.SetActive(true);
       
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(RemoveDoors))
        {
            DoorOne.SetActive(false);
            DoorTwo.SetActive(false);
        }
    }
 
    private void OnTriggerExit(Collider other)
    {
        Hint.gameObject.SetActive(false); 
    }
}
