using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extensions
{
    #region FUNCIONES

    /// <summary>
    /// Metodo de extensión que devuelve convierete un float de segundos a un int de milisegundos
    /// </summary>
    /// <param name="secs"></param>
    /// <returns></returns>
    public static int ToMillis(this float secs)
    {
        return (int)(secs * 1000);
    }

    public static string ToEnumFormat(this string str)
    {
        return str.Trim(' ').Replace(' ', '_');
    }

    public static string FromEnumFormat(this string str)
    {
        return str.Replace('_', ' ');
    }

    public static void CopyFrom(this AudioSource destination, AudioSource original)
    {
        destination.clip = original.clip;
        destination.volume = original.volume;
        destination.pitch = original.pitch;
        destination.loop = original.loop;
        destination.playOnAwake = false;
        destination.spatialBlend = 0; //Sonidos 2D
        destination.outputAudioMixerGroup = original.outputAudioMixerGroup;
    }

    public static IEnumerator ExecuteNextFrame<T>(Func<T> funct)
    {
        yield return new WaitForEndOfFrame();
        funct.Invoke();
    }

    #endregion
}