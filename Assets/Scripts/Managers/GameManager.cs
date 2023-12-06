using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    
    public enum State
    {
        Menu,
        Game,
        Win,
        Lose
    }
    private State _gameState;
    private int _connectedPlayers;

    [Header("Events")]
    public static Action<State> onGameStateChanged;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
    }

    private void NetworkManager_OnServerStarted()
    {
        if (!IsServer)
        {
            return;
        }
        
        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback;
    }

    private void Singleton_OnClientConnectedCallback(ulong obj)
    {
        _connectedPlayers++;
        if (_connectedPlayers>=2)
        {
            StartGame();
        }
    }
    
    private void Start()
    {
        _gameState = State.Menu;
    }
    
    private void StartGame()
    {
        StartGameClientRpc();
    }

    [ClientRpc]
    private void StartGameClientRpc()
    {
        _gameState = State.Game;
        onGameStateChanged?.Invoke(_gameState);
    }

    public void SetGameState(State state)
    {
        _gameState = state;
        onGameStateChanged?.Invoke(_gameState);
    }
}
