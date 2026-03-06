using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public int SceneNumb;

    public void SceneLoad()
    {
        SceneManager.LoadScene(SceneNumb);
    }
}
