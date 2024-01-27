using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class AnyButtonToNextScene : MonoBehaviour
{
    public string nextScene;

    public void SendToNextScene(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Debug.Log("Checking for nextScene");
        if (nextScene == "") {Debug.Log("nextScene not found!"); return;}
        SceneManager.LoadScene(nextScene);
    }

}
