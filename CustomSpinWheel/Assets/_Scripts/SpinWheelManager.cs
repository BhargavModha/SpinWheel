using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinWheelManager : MonoBehaviour
{
    public static SpinWheelManager instance;

    // events
    public event Action onSpinStart;
    public event Action<SpinWheelItem> onSpinEnd;
    public event Action onSpinReset;


    private void Awake(){
        instance = this;
    }

    public void SpinStart(){
        onSpinStart?.Invoke();
    }

    public void SpinEnd(SpinWheelItem item){
        onSpinEnd?.Invoke(item);
    }

    public void SpinReset(){
        onSpinReset?.Invoke();
    }

}
