﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Threading.Tasks;

[System.Serializable]
public class Sound
{
    public enum MixerType
    {
        SFX,
        Music
    }

    [HideInInspector]
    public AudioSource source;

    [PropertyOrder(-2), HorizontalGroup("clip"), HideLabel]
    public string name = "";
    [SerializeField][HideInInspector]
    private AudioClip _clip;
    [ShowInInspector, PropertyOrder(-1), HorizontalGroup("clip", LabelWidth = 40), HideLabel]
    public AudioClip clip
    {
        get => _clip;
        set {
            _clip = value;
            if (name == "" && _clip)
                name = _clip.name;
        }
    }

    [FoldoutGroup("gr1", GroupName ="Properties", Expanded = false, VisibleIf = "_clip")]
    [Range(0f,1f)]
    public float volume = 0.6f;
    [FoldoutGroup("gr1")][Range(.1f, 3f)]
    public float pitch = 1;
    [FoldoutGroup("gr1")]
    public MixerType mixer = MixerType.SFX;
    [FoldoutGroup("gr1")]
    public bool loop = false;
    [FoldoutGroup("gr1")][Range(0, 1.5f)]
    public float pitchRandMaxOffset = 0;

    [HideInInspector]
    public float originalPitch;

    [HideInInspector]
    public bool playable = true;

    public async Task StartPlayingOffset(float secs)
    {
        if (!playable)
            return;

        playable = false;
        await Task.Delay(secs.ToMillis());
        playable = true;
    }
}
