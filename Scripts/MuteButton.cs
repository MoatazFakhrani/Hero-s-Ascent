using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour{
    private static MuteButton instance;
    public static MuteButton MyInstance{
        get{
            if(instance == null){
                instance = FindObjectOfType<MuteButton>();
            }
            return instance;
        }
    }
    
    private bool isMuted = false;
    private Button button;

    private void Start(){
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleMute);
    }

    private void ToggleMute(){
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;
    }
}