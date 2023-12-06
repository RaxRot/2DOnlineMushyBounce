using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int _hostScore;
    private int _clientScore;
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
    }

    private void Start()
    {
        UpdateScoreText();
        UpdateScoreClientRpc(_hostScore,_clientScore);
    }

    private void NetworkManager_OnServerStarted()
    {
        if (!IsServer)
        {
            return;
        }

        Egg.onFellInWater += EggFellInWaterCallback;
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
        Egg.onFellInWater -= EggFellInWaterCallback;
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameManager.State state)
    {
        switch (state)
        {
            case GameManager.State.Game:
                ResetScores();
                break;
        }
    }

    private void ResetScores()
    {
        _hostScore = 0;
        _clientScore = 0;
        
        UpdateScoreClientRpc(_hostScore,_clientScore);
        UpdateScoreText();
    }

    private void EggFellInWaterCallback()
    {
        if (PlayerSelector.Instance.IsHostTurn())
        {
            _clientScore++;
        }
        else
        {
            _hostScore++;
        }
        
        UpdateScoreClientRpc(_hostScore,_clientScore);
        UpdateScoreText();

        CheckForEndGame();
    }

    private void CheckForEndGame()
    {
        if (_hostScore>=3)
        {
            HostWin();
        }
        else if(_clientScore>=3)
        {
            ClientWin();
        }
        else
        {
            //respawn
            ReuseEgg();
        }
    }
    
    private void HostWin()
    {
        HostWinClientRpc();
    }

    [ClientRpc]
    private void HostWinClientRpc()
    {
        if (IsServer)
        {
            GameManager.Instance.SetGameState(GameManager.State.Win);
        }
        else
        {
            GameManager.Instance.SetGameState(GameManager.State.Lose);
        }
    }

    private void ClientWin()
    {
        WinClientRpc();
    }

    [ClientRpc]
    private void WinClientRpc()
    {
        if (IsServer)
        {
            GameManager.Instance.SetGameState(GameManager.State.Lose);
        }
        else
        {
            GameManager.Instance.SetGameState(GameManager.State.Win);
        }
    }

    private void ReuseEgg()
    {
        EggManager.Instance.ReuseEgg();
    }

    private void UpdateScoreText()
    {
        UpdateScoreTextClientRpc();
    }

    [ClientRpc]
    private void UpdateScoreClientRpc(int hostScore,int clientScore)
    {
        _hostScore = hostScore;
        _clientScore = clientScore;
    }

    [ClientRpc]
    private void UpdateScoreTextClientRpc()
    {
        scoreText.text="<color=#0055ffff>" + _hostScore +"</color> - <color=#ff5500ff>"+ _clientScore +"</color >";
    }
}
