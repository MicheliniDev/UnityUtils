using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Unity/Audio Source")]
    public class AudioSourceVariable : FsmVariable<AudioSource> { }

    [Serializable] public class AudioSourceReference : ComponentReference<AudioSource> { }
}