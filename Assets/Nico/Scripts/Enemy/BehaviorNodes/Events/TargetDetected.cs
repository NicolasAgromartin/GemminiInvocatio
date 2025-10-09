using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/TargetDetected")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "TargetDetected", message: "[TargetSelected]", category: "Events", id: "a848b79fc933ea9b8a2d43e09110725f")]
public sealed partial class TargetDetected : EventChannel<GameObject> { }

