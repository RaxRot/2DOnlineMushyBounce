using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [Header("Physics Settings")]
    private Rigidbody2D _rb;
    [SerializeField] private float bounceVelocity;
    private bool _isAlive = true;
    private float _gravityScale;

    [Header("Events")]
    public static Action onHit;
    public static Action onFellInWater;
    public static Action onBumb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _gravityScale = _rb.gravityScale;
        _rb.gravityScale = 0;

        StartCoroutine("WaitAndFall");
    }
    
    private IEnumerator WaitAndFall()
    {
        yield return new WaitForSeconds(2f);

        _rb.gravityScale = _gravityScale;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!_isAlive)
        {
            return;
        }
        
        if (col.collider.TryGetComponent(out PlayerController playerController))
        {
            Bounce(col.GetContact(0).normal);
            onHit?.Invoke();
            Bumb();
        }
    }

    private void Bumb()
    {
        BumbClientRpc();
    }
    
[ClientRpc]
    private void BumbClientRpc()
    {
        onBumb?.Invoke();
    }

    private void Bounce(Vector2 normal)
    {
        _rb.velocity = normal * bounceVelocity;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!_isAlive)
        {
            return;
        }
        
        if (col.CompareTag("DeadZone"))
        {
            _isAlive = false;
            onFellInWater?.Invoke();
        }
    }

    public void Reuse()
    {
        transform.position=Vector2.up*5;
        _rb.velocity=Vector2.zero;
        _rb.angularVelocity = 0;
        _rb.gravityScale = 0;

        _isAlive = true;

        StartCoroutine("WaitAndFall");
    }
}
