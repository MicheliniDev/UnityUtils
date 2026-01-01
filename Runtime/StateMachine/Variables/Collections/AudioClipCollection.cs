using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Unity/Audio Clip")]
    public class AudioClipVariable : FsmVariable<AudioClip> { }

    [Serializable]
    public class AudioClipReference : FsmReference<AudioClip, AudioClipVariable> { }
}