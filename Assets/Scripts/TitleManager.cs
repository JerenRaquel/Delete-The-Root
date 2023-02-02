using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {
    public string self;
    public string newGameSceneName;
    public string loadGameSceneName;

    public void NewGame() {
        SceneHandler.instance.ReplaceScene(newGameSceneName, self);
    }

    public void LoadGame() {
        Debug.Log("Loading Game --Not Implemented");
    }
}
