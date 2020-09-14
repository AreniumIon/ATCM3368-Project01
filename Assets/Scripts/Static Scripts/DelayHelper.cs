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

    //same as above but when PlayerShip needs to be passed through
    //TODO: convert this to be more general (like with MonoBehaviour)
    public static Coroutine DelayAction(this MonoBehaviour monobehaviour, Action<PlayerShip> action, PlayerShip parameter, float delayDuration)
    {
        return monobehaviour.StartCoroutine(DelayActionRoutine(action, parameter, delayDuration));
    }

    private static IEnumerator DelayActionRoutine(Action<PlayerShip> action, PlayerShip parameter, float delayDuration)
    {
        yield return new WaitForSeconds(delayDuration);
        action(parameter);
    }
}
