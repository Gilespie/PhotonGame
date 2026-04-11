using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private float _initialForce;
    [SerializeField] private int _damage;
    [SerializeField] private float _lifeTime;

    //TickTimer para manejar timers de manera eficiente en red
    private TickTimer _lifeTimer;
    private NetworkRigidbody3D _rb;

    public override void Spawned()
    {
        _rb = GetComponent<NetworkRigidbody3D>();
        //Aplicar una fuerza al rigidbody
        _rb.Rigidbody.AddForce(transform.forward * _initialForce, ForceMode.Impulse);
        //crear el timer
        _lifeTimer = TickTimer.CreateFromSeconds(Runner, _lifeTime);
    }

    public override void FixedUpdateNetwork()
    {
        if (_lifeTimer.ExpiredOrNotRunning(Runner))
            return;
        //Chequear si el timer expiro
        
        Runner.Despawn(Object);
        //eliminar la bala si paso el timer
    }

    private void OnTriggerEnter(Collider other)
    {
        //aplicar daño al personaje con el que colisiono la bala
        if(other.TryGetComponent(out Player player))
        {
            player.RPC_TakeDamage(_damage);
        }

        //eliminar la bala al colisionar
        Runner.Despawn(Object);
    }
}
