using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiSpawnerMediator : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabsForInstanciate;
    [SerializeField] private List<EnemyInGame> enemiesInstatiate;
    [SerializeField] private EnemyInGame enemyTemplate;
    [SerializeField] private float speed;
    [SerializeField] private EliminadorDeObjetosPuntuacion eliminador;
    [SerializeField] private List<GameObject> listOfPoints;
    private float detaTimeLocal;
    private float timeInterval;
    private bool hasUse;
    private IMediator _mediator;

    public void Configuration(IMediator mediator)
    {
        hasUse = true;
        _mediator = mediator;
    }
    private void Update()
    {
        if (!hasUse) return;
        if (detaTimeLocal >= timeInterval)
        {
            detaTimeLocal = 0;
            timeInterval = Random.Range(1, 5);
            _mediator.Write("Spawn!");
            var enemyInGame = Instantiate(enemyTemplate, transform);
            var point = listOfPoints[Random.Range(0, listOfPoints.Count)];
            enemyInGame.transform.position = point.transform.position;
            enemyInGame.transform.rotation = point.transform.rotation;
            enemyInGame.Configurate(prefabsForInstanciate[Random.Range(0, prefabsForInstanciate.Count)],
                Random.Range(.1f, speed));
            
            enemyInGame.OnEnemyDestroy += () =>
            {
                enemiesInstatiate.Remove(enemyInGame);
            };
            enemyInGame.OnEnemyDestroyLegal += () =>
            {
                //_mediator.RestarVida(10);
                enemiesInstatiate.Remove(enemyInGame);
            };
            enemiesInstatiate.Add(enemyInGame);
        }
        detaTimeLocal += Time.deltaTime;
    }

    public EliminadorDeObjetosPuntuacion GetDeletedEnemies()
    {
        return eliminador;
    }

    public void StopSpawn()
    {
        hasUse = false;
    }

    public void CleanEnemies()
    {
        foreach (var enemyInGame in enemiesInstatiate)
        {
            Destroy(enemyInGame.gameObject, 2);
        }

        enemiesInstatiate = new List<EnemyInGame>();
    }
}