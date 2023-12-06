using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSelector : NetworkBehaviour
{
    public static PlayerSelector Instance;
    
    private bool _isHostTurn;

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
        
        GameManager.onGameStateChanged += GameStateChangedCallback;
        Egg.onHit += SwitchPlayers;
    }

    public override void OnDestroy()
    {
        NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
        GameManager.onGameStateChanged -= GameStateChangedCallback;
        Egg.onHit -= SwitchPlayers;
    }

    private void GameStateChangedCallback(GameManager.State gameState)
    {
        switch (gameState)
        {
            case GameManager.State.Game:
                Initialize();
                break;
        }
    }

    private void Initialize()
    {
        //Look for every player in game 
        PlayerStateManager[] playerStateManagers = FindObjectsOfType<PlayerStateManager>();
        for (int i = 0; i < playerStateManagers.Length; i++)
        {
            if (playerStateManagers[i].GetComponent<NetworkObject>().IsOwnedByServer)
            {
                //This is host
                //if it host turn,enable host
                //disable client
                if (_isHostTurn)
                {
                    playerStateManagers[i].Enable();
                }
                else
                {
                    playerStateManagers[i].Disable();
                }
            }
            else
            {
                if (_isHostTurn)
                {
                    playerStateManagers[i].Disable();
                }
                else
                {
                    playerStateManagers[i].Enable();
                }
            }
        }
    }
    
    private void SwitchPlayers()
    {
        _isHostTurn = !_isHostTurn;
        Initialize();
    }

    public bool IsHostTurn()
    {
        return _isHostTurn;
    }
}
