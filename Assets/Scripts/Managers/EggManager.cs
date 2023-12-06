using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EggManager : NetworkBehaviour
{
    public static EggManager Instance;
    
    [Header("Elements")]
    [SerializeField] private Egg eggPrefab;

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

    private void OnEnable()
    {
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameManager.State gameState)
    {
        switch (gameState)
        {
            case GameManager.State.Game:
                SpawnEgg();
                break;
        }
    }

    private void SpawnEgg()
    {
        if (!IsServer)
        {
            return;
        }
        
        Egg eggInstance = Instantiate(eggPrefab, Vector3.up * 5, Quaternion.identity,transform);
        eggInstance.GetComponent<NetworkObject>().Spawn();
        eggInstance.transform.SetParent(transform);
    }

    public void ReuseEgg()
    {
        if (!IsServer)
        {
            return;
        }

        if (transform.childCount<=0)
        {
            return;
        }

        transform.GetChild(0).GetComponent<Egg>().Reuse();
    }
}
