using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {
    #region Class Instance
    public static SceneHandler instance = null;
    private void CreateInstance() {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion
    private void Awake() => CreateInstance();

    public string startingScene;
    public string self;

    private HashSet<string> loadedScenes;

    private void Start() {
        loadedScenes = new HashSet<string>();
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            if (SceneManager.GetSceneAt(i).name != self) {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
            }
        }

        if (!this.loadedScenes.Contains(startingScene)) {
            LoadScene(startingScene);
        }
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        loadedScenes.Add(sceneName);
    }

    public void ReplaceScene(string newScene, string loadedScene) {
        if (newScene == loadedScene) return;

        UnloadScene(loadedScene);
        LoadScene(newScene);
    }

    public void UnloadScene(string sceneName) {
        if (sceneName == self) return;
        if (!loadedScenes.Contains(sceneName)) return;

        SceneManager.UnloadSceneAsync(sceneName);
        this.loadedScenes.Remove(sceneName);
    }

    public bool IsLoaded(string sceneName) {
        return this.loadedScenes.Contains(sceneName);
    }
}
