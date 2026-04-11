using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private GameObject _playerPrefab;

    //Spawn points para los distintos players
    [SerializeField] private Transform[] _spawnTransforms;

    //bool interno que indica si ya hay la cantidad de jugadores necesaria para jugar (2 segun la consigna)
    [SerializeField] int _minPlayersCount = 2;
    private bool _initialized;
    
    //Se ejecuta por CADA cliente conectado
    public void PlayerJoined(PlayerRef player)
    {
        var playersCount = Runner.SessionInfo.PlayerCount;
        
        //si el primer cliente ya espero al 2do spawneo un prefab en la 1era posicion
        if (_initialized && playersCount >= _minPlayersCount)
        {
            CreatePlayer(0);
            return;
        }
        
        //Si el cliente que entro, es el mismo cliente donde corre este codigo, entonces:
        if (player == Runner.LocalPlayer)
        {
            //si el player count es menor a la cantidad minima, seteo initialized.
            //sino crear un player en el spawn point correspondiente
            if (playersCount < _minPlayersCount)
            {
                _initialized = true;
            }
            else
            {
                CreatePlayer(1);
            }
            
        }
    }

    void CreatePlayer(int spawnPointIndex)
    {
        _initialized = false;

        //consigo la posicion y rotacion del spawn point correspondiente
        Runner.Spawn(_playerPrefab, _spawnTransforms[spawnPointIndex].position, Quaternion.identity);
        
        //spawneo el prefab en la posicion y rotacion correcta
    }
}
