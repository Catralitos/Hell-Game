using UnityEngine;
using UnityEngine.Localization;

namespace Dialogues.ScriptableObjects
{
    public enum ActorID
    {
        Protagonist,
        TestNPC,
        Leonard,
        Agathe,
        Jilaiya,
        Fluffy,
        Petey
    }
    
    [CreateAssetMenu(menuName = "Dialogues/Actor")]
    public class ActorSO : ScriptableObject
    {
        public ActorID actorId;
        public LocalizedString actorName;
    }
}