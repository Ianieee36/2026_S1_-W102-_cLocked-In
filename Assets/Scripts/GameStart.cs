using Unity.Collections;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
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
        SceneManager.LoadScene("Map");
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        SceneManager.LoadScene("Player", LoadSceneMode.Additive);
        SceneManager.LoadScene("Boss", LoadSceneMode.Additive);
    }
}
