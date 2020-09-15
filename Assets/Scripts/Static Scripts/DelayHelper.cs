using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DelayHelper
{
    //call this when an action is supposed to be delayed by delayDuration before its effects trigger
    public static Coroutine DelayAction(this MonoBehaviour monobehaviour, Action action, float delayDuration)
    {
        return monobehaviour.StartCoroutine(DelayActionRoutine(action, delayDuration));
    }

    private static IEnumerator DelayActionRoutine(Action action, float delayDuration)
    {
        yield return new WaitForSeconds(delayDuration);
        action();
    }

    //same as above but when parameter needs to be passed through
    public static Coroutine DelayAction<T>(this MonoBehaviour monobehaviour, Action<T> action, T parameter, float delayDuration)
    {
        return monobehaviour.StartCoroutine(DelayActionRoutine(action, parameter, delayDuration));
    }

    private static IEnumerator DelayActionRoutine<T>(Action<T> action, T parameter, float delayDuration)
    {
        yield return new WaitForSeconds(delayDuration);
        action(parameter);
    }
}
