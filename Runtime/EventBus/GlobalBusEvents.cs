using MicheliniDev.Utils;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Global Bus Events", fileName = "New Global Bus Events")]
public class GlobalBusEvents : ScriptableSingleton<GlobalBusEvents>
{
    public List<string> EventNames = new List<string>();
}