using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public enum GameState
    {
        Waiting,
        Fighting
    }

    private float _newWaveTime = 10f;
    private float _newWaveTimer = 10f;    
    
    private float _cooldownTime = 2f;
    private float _cooldownTimer = 2f;
    private bool colldown = true;


    private bool _setNewWave = true;

    public int _aliveEnemies = 0;
    public int _deadEnemies = 0;


    public static GameControl Instance { get; private set; }

    public GameState _currentGameState;

    [SerializeField] private List<GameObject> Waves;

    private int index = 0;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _currentGameState = GameState.Waiting;
    }

    private void Update()
    {
        if (colldown)
        {
            if (_cooldownTimer <= 0)
            {
                _cooldownTimer = _cooldownTime;
                colldown = false;
            } else
            {
                _cooldownTimer -= Time.deltaTime;
                Debug.Log("aaaaaaaaaa");
                return;
            }
        }

        if (_setNewWave)
        {   

            
            

            if (_newWaveTimer <= 0)
            {
                _newWaveTimer = _newWaveTime;
                _setNewWave = false;
                SetNewWave();
                colldown = true;
            } else
            {
                _newWaveTimer -= Time.deltaTime;
            }
        }

        if (!_setNewWave && _aliveEnemies == 0)
        {
            _setNewWave = true;
            EndWave();
        }

    }

    private void SetNewWave()
    {
        Debug.Log("sasa");
        Waves[index].SetActive(true);
        _currentGameState = GameState.Fighting;
        index++;
    }

    private void EndWave()
    {
        Waves[index].SetActive(false);
        if (index - 1 == Waves.Count)
        {   

            Debug.Log("йнмеж");
            return;

        }
        _setNewWave = true;
        _currentGameState = GameState.Waiting;
    }

    public bool IsFighting()
    {
        return _currentGameState == GameState.Fighting;
    }
}
