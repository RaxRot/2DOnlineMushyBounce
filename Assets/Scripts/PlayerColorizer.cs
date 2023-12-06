using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerColorizer : NetworkBehaviour
{
    [Header("Elements")]
    [SerializeField] private SpriteRenderer[] renderers;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsServer && IsOwner)
        {
            ColorizeServerRpc(Color.red);
        }
    }

    [ServerRpc]
    private void ColorizeServerRpc(Color color)
    {
        ColorizeClientRpc(color);
    }
    [ClientRpc]
    private void ColorizeClientRpc(Color color)
    {
        foreach (var renderer in renderers)
        {
            renderer.color = color;
        }
    }
}
