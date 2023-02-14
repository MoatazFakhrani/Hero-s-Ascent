using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour{

    bool restart;
    private static MainMenu instance;

    public static MainMenu MyInstance{
        get{
            if(instance == null){
                instance = FindObjectOfType<MainMenu>();
            }
            return instance;
        }
    }

    void Awake(){
        
        DontDestroyOnLoad(this.gameObject);
        restart = false;
    }

    public void PlayGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UIManager.MyInstance.OpenCloseMenu();
    }

    public void OnPlayerDeath(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResumeGame(){
        UIManager.MyInstance.OpenCloseMenu();
    }

    public void MainMenuScene(){
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame(){
        Debug.Log("QUIT!");
        Application.Quit();
    }
}