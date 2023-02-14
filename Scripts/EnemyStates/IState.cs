using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState{

    //Get the state ready
    void Enter(Enemy parent);

    void Update();

    void Exit();
}
