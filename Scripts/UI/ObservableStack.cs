using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate void UpdateStackEvent();

//Observable stack inherets from Stack
class ObservableStack<T> : Stack<T>{

    //Events for Push stack, Pop stack, and Clear stack
    public event UpdateStackEvent OnPush;
    public event UpdateStackEvent OnPop;
    public event UpdateStackEvent OnClear;

    public new void Push(T item){

        //Call the base function from the superclass
        base.Push(item);

        if(OnPush != null){

            OnPush();
        }
    }
    
    
    public new T Pop(){

        //Remove the item, T means type
        T item = base.Pop();

        if(OnPop != null){
            OnPop();
        }

        return item;
    }

     public new void Clear(){

        base.Clear();

        if(OnClear != null){
            OnClear();
        }
    }
}
