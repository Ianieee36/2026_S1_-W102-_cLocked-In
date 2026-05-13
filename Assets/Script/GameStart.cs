using Unity.Collections;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{

    public bool disableMainMenu = true; // set to true if you want to skip loading the main menu (for testing purposes)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bool testing = true; // change to false if you dont wan't all the scenes to load

        if (testing)
        {
            StartGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // On start buttons pressed run this,
    // loads all the scenes we need.
    public void StartGame()
    {
        if (disableMainMenu)
        {
            SceneManager.LoadScene("Map");
            SceneManager.LoadScene("Player", LoadSceneMode.Additive);
            SceneManager.LoadScene("Boss", LoadSceneMode.Additive);
            SceneManager.LoadScene("NPCs", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene("Map");
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
            SceneManager.LoadScene("Player", LoadSceneMode.Additive);
            SceneManager.LoadScene("Boss", LoadSceneMode.Additive);
            SceneManager.LoadScene("AccessibilityMenu", LoadSceneMode.Additive);
            SceneManager.LoadScene("AudioMenu", LoadSceneMode.Additive);
            SceneManager.LoadScene("GameSettingsMenu", LoadSceneMode.Additive);
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
            SceneManager.LoadScene("NPCs", LoadSceneMode.Additive);
        }
    }
}
