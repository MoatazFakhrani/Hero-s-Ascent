using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour{
    private bool isMuted = false;
    private void ToggleMute(){
        isMuted = ! isMuted;
        AudioListener.volume = isMuted ? 0 : 1;
    }
}
