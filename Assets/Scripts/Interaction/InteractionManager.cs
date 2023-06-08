using System.Collections.Generic;
using Events.ScriptableObjects;
using Player;
using UnityEngine;

namespace Interaction
{
    public enum InteractionType { None = 0, Talk };

    public class InteractionManager : MonoBehaviour
    {
        //Events for the different interaction types
        /*[Header("Broadcasting on")]
        public DialogueActorChannelSO startTalking;
        public InteractionUIEventChannelSO toggleInteractionUI ;

        [Header("Listening to")]
        public VoidEventChannelSO onInteractionEnded;
	
        public InteractionType currentInteractionType; //This is checked/consumed by conditions in the StateMachine

        private LinkedList<Interaction> potentialInteractions = new LinkedList<Interaction>(); 
        
        private void OnEnable()
        {
            //PlayerEntity.Instance. += OnInteractionButtonPress;
            onInteractionEnded.OnEventRaised += OnInteractionEnd;
        }

        private void OnDisable()
        {
            onInteractionEnded.OnEventRaised -= OnInteractionEnd;
        }*/
    }
}