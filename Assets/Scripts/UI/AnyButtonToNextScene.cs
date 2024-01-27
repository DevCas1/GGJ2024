using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class AnyButtonToNextScene : MonoBehaviour
{
    public Scene nextScene;

    public void SendToNextScene()
    {
        if (nextScene == null) return;
        SceneManager.LoadScene(nextScene.name);
    }

}
