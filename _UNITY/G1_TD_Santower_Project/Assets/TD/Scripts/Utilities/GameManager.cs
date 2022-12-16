using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public enum GamePhase
    {
        Intro,
        Phase1,
        Phase2,
        Phase3,
        Phase4,
        GameWin,
        GameLose,
        None,
    }

    [SerializeField]
    private GamePhase _currentPhase = GamePhase.None;

    public UnityEvent<GamePhase, GamePhase> GamePhaseChangeEvent_UE;

    public void ChangePhase(GamePhase toPhase)
    {
        GamePhaseChangeEvent_UE.Invoke(_currentPhase, toPhase);
        _currentPhase = toPhase;

        switch (toPhase)
        {
            case GamePhase.Intro:
                //This phase is reserved for the start of the game, the tutorial and intro.
                break;
            case GamePhase.Phase1:
                //This phase is reserved for waves 1-5, ended by the NutCracker.
                break;
            case GamePhase.Phase2:
                //This phase is reserved for waves 6-10, ended by the Merry a Carrier.
                break;
            case GamePhase.Phase3:
                //This phase is reserved for waves 11-15, ended by the MechaSnowman.
                break;
            case GamePhase.Phase4:
                //This phase is reserved for waves 16-20, ended by the Father Fouettard.
                break;
            case GamePhase.GameWin:
                //This phase is reserved for after defeating Father Fouettard.
                Victory();
                break;
            case GamePhase.GameLose:
                //This phase is reserved for if the player loses all base health.
                Defeat();
                break;
            case GamePhase.None:
                //When not in game.
                break;
            default:
                break;
        }

        Debug.Log("Game phase changed to " + toPhase);
    }

    private void Defeat()
    {
        Time.timeScale = 0;
    }

    private void Victory()
    {
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        Loader.Load(Loader.Scene.Level_One);
    }

    public void ReturnToMainMenu()
    {
        ChangePhase(GamePhase.None);
        Time.timeScale = 1;
        Loader.Load(Loader.Scene.MainMenu);
    }

}
