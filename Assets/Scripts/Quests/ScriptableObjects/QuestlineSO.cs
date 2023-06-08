using System.Collections.Generic;
using Events.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Quests/Questlines")]
    public class QuestlineSO : ScriptableObject
    {
        
        public int idQuestLine;
        
        [Tooltip("The collection of Quests composing the Questline")]
        public List<QuestSO> quests = new List<QuestSO>();
        public bool isDone;
        VoidEventChannelSO endQuestlineEvent;
        
        public void FinishQuestline()
        {
            if(endQuestlineEvent!=null)
            { endQuestlineEvent.RaiseEvent();  }
            isDone = true;
        }
        
        public void SetQuestlineId(int id)
        {
            idQuestLine = id;
        }
#if UNITY_EDITOR
        /// <summary>
        /// This function is only useful for the Questline Tool in Editor to remove a Questline
        /// </summary>
        /// <returns>The local path</returns>
        public string GetPath()
        {
            return AssetDatabase.GetAssetPath(this);
        }
#endif
    }
}