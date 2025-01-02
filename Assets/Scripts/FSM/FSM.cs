using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    State current;
    public FSM(State initialState)
    {
        current = initialState;
        current.Enter();
    }

    //Transition of States
    public void SendInput(string input)
    {
        if (current == null) return;

        if(current.HasInput(input))
        {
            State next = current.GetState(input);

            if (next == null) throw new System.Exception("Error. No hay estado");
            current.Exit();
            current = next;
            current.Enter(); 
        }
    }

    public void FsmUpdate(float delta)
    {
        if(current != null)
        {
            current.Update(delta);
        }
    }
}

