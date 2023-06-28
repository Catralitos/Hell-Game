using UnityEngine;

namespace Gameplay.ScriptableObjects
{
    public enum GameState
    {
        Gameplay, //regular state: player moves, attacks, can perform actions
        Pause, //pause menu is opened, the whole game world is frozen
        Inventory, //when inventory UI or cooking UI are open
        Dialogue
    }

    [CreateAssetMenu(menuName = "Gameplay/Game State")]
    public class GameStateSO : ScriptableObject
    {
    
        [Header("Game states")]
        public GameState currentGameState;
        public GameState previousGameState;

        public void Init()
        {
            currentGameState = GameState.Gameplay;
            previousGameState = GameState.Pause;
        }
        
        public void UpdateGameState(GameState newGameState)
        {
            if (newGameState == currentGameState)
                return;
        
            previousGameState = currentGameState;
            currentGameState = newGameState;
        }

        public void ResetToPreviousGameState()
        {
            if (previousGameState == currentGameState)
                return;
        
            (previousGameState, currentGameState) = (currentGameState, previousGameState);
        }
    }
}