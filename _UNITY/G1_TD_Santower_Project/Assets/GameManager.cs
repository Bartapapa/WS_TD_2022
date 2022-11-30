using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    }

    public delegate void GamePhaseChangeEvent(GamePhase fromPhase, GamePhase toPhase);
    public event GamePhaseChangeEvent _gamePhaseChanged = null;


    [SerializeField]
    private GamePhase _currentPhase = GamePhase.Intro;

    public void ChangePhase(GamePhase toPhase)
    {
        _gamePhaseChanged?.Invoke(_currentPhase, toPhase);
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
                break;
            case GamePhase.GameLose:
                //This phase is reserved for if the player loses all base health.
                break;
            default:
                break;
        }

        Debug.Log("Game phase changed to " + toPhase);
    }


}
