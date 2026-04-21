using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace FoxShooter.Characters.AI
{
#if UNITY_EDITOR
    [CreateAssetMenu(menuName = "Behavior/Event Channels/Spotted Enemy")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "Spotted Enemy", message: "Agent has spotted [enemy] , transform [transform]", category: "Events", id: "3d723e9ace263fde6451421cdda6ad91")]
    public class SpottedEnemy : EventChannel<GameObject, Transform> { }
}