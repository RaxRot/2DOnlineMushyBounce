using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerStateManager : NetworkBehaviour
{
   [Header("Elements")]
   [SerializeField] private Collider2D collider;
   [SerializeField]private SpriteRenderer[] renderers;
   
   public void Enable()
   {
      EnableClientRpc();
   }

   [ClientRpc]
   private void EnableClientRpc()
   {
      collider.enabled = true;
      foreach (var renderer in renderers)
      {
         Color color = renderer.color;
         color.a = 1f;
         renderer.color = color;
      }
   }

   public void Disable()
   {
      DisableClientRpc();
   }

   [ClientRpc]
   private void DisableClientRpc()
   {
      collider.enabled = false;
      foreach (var renderer in renderers)
      {
         Color color = renderer.color;
         color.a = 0.3f;
         renderer.color = color;
      }
   }
}
