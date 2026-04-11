using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _jumpForce;
    [SerializeField] int _maxLife;
    [Networked] float Health {  get; set; }
    [Networked]  public float _currentHealth { get; private set; }
    //Crear una variable networkeada de vida actual y que tenga una forma de debugear el valor de vida cada vez que este se actualice

    //crear referencia a la bala y a una posicion para spawnear balas.
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnerTransform;

    [Header("Input Keys")]
    public KeyCode _jumpKey;
    public KeyCode _shootKey;
    
    private bool _isJumpPressed;
    private bool _isShootPressed;
    
    private float _horizontalInput;

    //crear variable de rigidbody
    private NetworkRigidbody3D _rb;
    
    
    public override void Spawned()
    {
        //conseguir rigidbody
        _rb = GetComponent<NetworkRigidbody3D>();

        //setear vida actual igual a la vida maxima
        _currentHealth = _maxLife;
        
        //Setear el follow target al cumplir la condicion correcta (piensen cual puede ser esa condicion)
        

        GameManager.Instance.AddToList(this);
    }
    
    void Update()
    {
        if (!HasStateAuthority) return;
        
        _horizontalInput = Input.GetAxis("Horizontal");

        //Chequear input para ejecutar el salto
        _isJumpPressed = Input.GetKeyDown(_jumpKey);

        //Chequear input para ejecutar el disparo
        _isShootPressed = Input.GetKeyDown(_shootKey);
    }

    public override void FixedUpdateNetwork()
    {
        //llamamos a nuestras funciones de accion en el FixedUpdateNetwork
        Movement(_horizontalInput);

        if (_isJumpPressed)
        {
            Jump();
            
            _isJumpPressed = false;
        }
        
        if (_isShootPressed)
        {
            SpawnShot();
            
            _isShootPressed = false;
        }
    }

    void Movement(float xAxi)
    {
        if (xAxi != 0)
        {
            //roto el transform hacia donde me estoy moviendo
            transform.forward = Vector3.right * Mathf.Sign(xAxi);
            
            //muevo a traves del rigidbody
            

            if (Mathf.Abs(_rb.Rigidbody.linearVelocity.z) > _speed)//Clampeo la velocidad
            {
                var velocity = Vector3.ClampMagnitude(_rb.Rigidbody.linearVelocity, _speed);

                velocity.y = _rb.Rigidbody.linearVelocity.y;

                _rb.Rigidbody.linearVelocity = velocity;
            }
        }

    }

    void Jump()
    {
        //aplico fuerza al rigidbody
        _rb.Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Acceleration);
    }

    void SpawnShot()
    {
        //spawneo la bala
        Runner.Spawn(_bulletPrefab, _bulletSpawnerTransform.position, Quaternion.identity);
    }

    //Hacer una funcion local para recibir daño y que llame a la funcion de morir cuando la vida sea <= 0
    void TakeDamage(float damage)
    {
        if (damage <= 0) return;

        _currentHealth -= damage;

        _currentHealth = MathF.Min(_currentHealth - damage, _maxLife);

        if (_currentHealth <= 0)
        {
            Death();
        }
    }


    //Hacer una funcion networkeada para recibir daño que llame a la funcion local de recibir daño.
    [Rpc]
    public void RPC_TakeDamage(float damage)
    {
        TakeDamage(damage);
    }

    void Death()
    {
        Debug.Log("Mori :(");

        //Llamo a la funcion de derrota del game manager y paso mi local player
        GameManager.Instance.RPC_Defeat(Runner.LocalPlayer);

        Runner.Despawn(Object);
    }
}