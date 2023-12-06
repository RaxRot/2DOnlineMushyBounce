using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
   [Header("Panels")] 
   [SerializeField] private GameObject connectionPanel;
   [SerializeField] private GameObject waitingPanel;
   [SerializeField] private GameObject gamePanel;
   [SerializeField] private GameObject winPanel;
   [SerializeField] private GameObject losePanel;

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
            ShowGamePanel();
            break;
         
         case GameManager.State.Win:
            ShowWinPanel();
            break;
         case GameManager.State.Lose:
            ShowLosePanel();
            break;
      }
   }

   private void Start()
   {
      ShowConnectionPanel();
   }

   private void ShowConnectionPanel()
   {
      connectionPanel.SetActive(true);
      waitingPanel.SetActive(false);
      gamePanel.SetActive(false);
      winPanel.SetActive(false);
      losePanel.SetActive(false);
   }

   private void ShowWaitingPanel()
   {
      connectionPanel.SetActive(false);
      waitingPanel.SetActive(true);
      gamePanel.SetActive(false);
   }

   private void ShowGamePanel()
   {
      connectionPanel.SetActive(false);
      waitingPanel.SetActive(false);
      gamePanel.SetActive(true);
   }

   public void HostButtonCallback()
   {
      NetworkManager.Singleton.StartHost();
      ShowWaitingPanel();
   }

   public void ClientButtonCallback()
   {
      NetworkManager.Singleton.StartClient();
      ShowWaitingPanel();
   }

   private void ShowWinPanel()
   {
      winPanel.SetActive(true);
   }

   private void ShowLosePanel()
   {
      losePanel.SetActive(true);
   }

   public void NextButtonCallback()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      NetworkManager.Singleton.Shutdown();
   }

   public void Test()
   {
      print("TEST");
   }
}
