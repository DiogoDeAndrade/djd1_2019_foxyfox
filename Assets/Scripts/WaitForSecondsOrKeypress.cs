using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForSecondsOrKeypress : IEnumerator
{
    float   startTime;
    KeyCode key;
    float   totalTime;

    public WaitForSecondsOrKeypress(float time, KeyCode inKey)
    {
        totalTime = time;
        startTime = Time.time;
        key = inKey;
    }

    object IEnumerator.Current
    {
        get { return Time.time - startTime; }
    }
    
    bool IEnumerator.MoveNext()
    {
        if ((Time.time - startTime) > totalTime)
        {
            return false;
        }
        if (Input.GetKey(key))
        {
            return false;
        }

        return true;
    }
   
    void IEnumerator.Reset()
    {

    }
    
}
