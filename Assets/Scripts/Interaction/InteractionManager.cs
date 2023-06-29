using System;
using System.Collections.Generic;
using Audio;
using Events.ScriptableObjects;
using Inputs;
using Inventory;
using Inventory.ScriptableObjects;
using Quests;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace Interaction
{
	public enum InteractionType { None = 0, PickUp, Talk }

	public class InteractionManager : MonoBehaviour
	{
		public InputReader inputReader;

		//Events for the different interaction types
		[Header("Broadcasting on")]
		public ItemSOEventChannelSO onObjectPickUp;
		public DialogueActorChannelSO startTalking;
		public InteractionUIEventChannelSO toggleInteractionUI;
		
		[Header("Listening to")]
		public VoidEventChannelSO onInteractionEnded;
	
		public InteractionType currentInteractionType; //This is checked/consumed by conditions in the StateMachine

		private LinkedList<Interaction> _potentialInteractions = new LinkedList<Interaction>(); //To store the objects we the player could potentially interact with

		private AudioManager _audioManager;

		private void Start()
		{
			_audioManager = GetComponent<AudioManager>();
		}

		private void OnEnable()
		{
			inputReader.InteractEvent += OnInteractionButtonPress;
			onInteractionEnded.OnEventRaised += OnInteractionEnd;
		}

		private void OnDisable()
		{
			inputReader.InteractEvent -= OnInteractionButtonPress;
			onInteractionEnded.OnEventRaised -= OnInteractionEnd;
		}
		
		private void Collect()
		{
			GameObject itemObject = _potentialInteractions.First.Value.interactableObject;
			_potentialInteractions.RemoveFirst();

			if (onObjectPickUp != null)
			{
				_audioManager.Play("ItemPickUp");
				ItemSO currentItem = itemObject.GetComponent<CollectableItem>().GetItem();
				onObjectPickUp.RaiseEvent(currentItem);
			}

			Destroy(itemObject); //TODO: maybe move this destruction in a more general manger, to implement a removal SFX

			RequestUpdateUI(false);
		}

		private void OnInteractionButtonPress()
		{
			if (_potentialInteractions.Count == 0)
				return;

			currentInteractionType = _potentialInteractions.First.Value.type;

			switch (_potentialInteractions.First.Value.type)
			{
				case InteractionType.Talk:
					if (startTalking != null)
					{
						_potentialInteractions.First.Value.interactableObject.GetComponent<StepController>().InteractWithCharacter();
						inputReader.EnableDialogueInput();
					}
					break;
               case InteractionType.PickUp:
	               Collect();
	               break;
			}
		}

		//Called by the Event on the trigger collider on the child GO called "InteractionDetector"
		public void OnTriggerChangeDetected(bool entered, GameObject obj)
		{
			if (entered)
				AddPotentialInteraction(obj);
			else
				RemovePotentialInteraction(obj);
		}

		private void AddPotentialInteraction(GameObject obj)
		{
			Interaction newPotentialInteraction = new Interaction(InteractionType.None, obj);

			if (obj.CompareTag("Pickable"))
			{
				newPotentialInteraction.type = InteractionType.PickUp;
			}
			else if (obj.CompareTag("NPC"))
			{
				newPotentialInteraction.type = InteractionType.Talk;
			}

			if (newPotentialInteraction.type != InteractionType.None)
			{
				_potentialInteractions.AddFirst(newPotentialInteraction);
				RequestUpdateUI(true);
			}
		}

		private void RemovePotentialInteraction(GameObject obj)
		{
			LinkedListNode<Interaction> currentNode = _potentialInteractions.First;
			while (currentNode != null)
			{
				if (currentNode.Value.interactableObject == obj)
				{
					_potentialInteractions.Remove(currentNode);
					break;
				}
				currentNode = currentNode.Next;
			}

			RequestUpdateUI(_potentialInteractions.Count > 0);
		}

		private void RequestUpdateUI(bool visible)
		{
			if (visible)
				toggleInteractionUI.RaiseEvent(true, _potentialInteractions.First.Value.type);
			else
				toggleInteractionUI.RaiseEvent(false, InteractionType.None);
		}

		private void OnInteractionEnd()
		{
			switch (currentInteractionType)
			{
				case InteractionType.Talk:
					//We show the UI after cooking or talking, in case player wants to interact again
					RequestUpdateUI(true);
					break;
			}

			inputReader.EnableGameplayInput();
		}

		private void ResetPotentialInteractions(PlayableDirector _playableDirector)
		{
			_potentialInteractions.Clear();
			RequestUpdateUI(_potentialInteractions.Count > 0);
		}
	}
}