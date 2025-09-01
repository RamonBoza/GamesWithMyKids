using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public List<GameObject> powerUpPrefabs = new List<GameObject>();

    [Header("Spawn Rules")]
    public int maxActive = 3;
    public float spawnEveryMin = 6f;
    public float spawnEveryMax = 10f;

    [Header("Area (XZ)")]
    public Vector3 areaCenter = Vector3.zero;
    public Vector3 areaSize = new Vector3(12f, 0f, 12f);

    [Header("Placement")]
    public LayerMask groundMask = ~0;     // qué capas se consideran "suelo"
    public float yCastHeight = 10f;       // altura desde la que lanzamos el raycast hacia abajo
    public float minDistanceToPlayers = 2f;

    [Header("Parent (optional)")]
    public Transform parentForSpawned;    // arrastra un empty "PowerUps"

    private readonly List<GameObject> active = new List<GameObject>();
    private float nextSpawnAt;

    void Start()
    {
        ScheduleNext();
    }

    void Update()
    {
        // Limpieza de referencias destruidas
        for (int i = active.Count - 1; i >= 0; i--)
        {
            if (active[i] == null) active.RemoveAt(i);
        }

        if (Time.time >= nextSpawnAt && active.Count < maxActive)
        {
            TrySpawnOne();
            ScheduleNext();
        }
    }

    void ScheduleNext()
    {
        float delay = Random.Range(spawnEveryMin, spawnEveryMax);
        nextSpawnAt = Time.time + delay;
    }

    void TrySpawnOne()
    {
        if (powerUpPrefabs.Count == 0) return;

        // Hasta 20 intentos de posición válida
        for (int tries = 0; tries < 20; tries++)
        {
            Vector3 posXZ = RandomPointInArea();
            Vector3 castFrom = new Vector3(posXZ.x, areaCenter.y + yCastHeight, posXZ.z);

            if (Physics.Raycast(castFrom, Vector3.down, out RaycastHit hit, yCastHeight * 2f, groundMask))
            {
                Vector3 spawnPos = hit.point + Vector3.up * 0.2f;

                if (TooCloseToPlayers(spawnPos)) continue;

                GameObject prefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Count)];
                GameObject go = Instantiate(prefab, spawnPos, Quaternion.identity, parentForSpawned);
                active.Add(go);

                // Notificar al power-up su spawner
                var pu = go.GetComponentInChildren<PowerUp>();
                if (pu != null) pu.SetSpawner(this);

                return; // éxito
            }
        }
        // Si no encontró hueco, ya lo intentará en el siguiente tic
    }

    Vector3 RandomPointInArea()
    {
        float x = Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f);
        float z = Random.Range(-areaSize.z * 0.5f, areaSize.z * 0.5f);
        return new Vector3(areaCenter.x + x, areaCenter.y, areaCenter.z + z);
    }

    bool TooCloseToPlayers(Vector3 pos)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in players)
        {
            if (Vector3.Distance(p.transform.position, pos) < minDistanceToPlayers)
                return true;
        }
        return false;
    }

    // Llamado por PowerUp cuando se recoge o expira
    public void NotifyPowerUpGone(PowerUp pu)
    {
		Debug.Log("Powerups activos: " + active.Count);
        // Quitar de la lista si existiera (por si acaso)
        for (int i = active.Count - 1; i >= 0; i--)
        {
            if (active[i] == null) { active.RemoveAt(i); continue; }
            if (active[i].GetComponentInChildren<PowerUp>() == pu)
            {
                active.RemoveAt(i);	
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.3f);
        Gizmos.matrix = Matrix4x4.TRS(areaCenter, Quaternion.identity, Vector3.one);
        Gizmos.DrawCube(Vector3.zero, new Vector3(areaSize.x, 0.01f, areaSize.z));
        Gizmos.color = new Color(0.2f, 0.8f, 1f, 1f);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(areaSize.x, 0.01f, areaSize.z));
    }
}
