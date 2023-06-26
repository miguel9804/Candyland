using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    public void ChangeScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }
}
