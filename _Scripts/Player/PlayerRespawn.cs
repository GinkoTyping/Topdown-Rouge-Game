using Ginko.PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ginko.CoreSystem;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField]
    public float respawningCountDown;
    private Stats stats;

    private Vector2 respawnPosition;
    private bool isRespawning = false;
    private float respawningTime;

    private void Start()
    {
        respawnPosition = GameObject.Find("Items").transform.Find("Respawn Point").transform.position;
        stats = Player.Instance.GetComponentInChildren<Core>().GetCoreComponent<Stats>();

        stats.Health.OnCurrentValueZero += StartRespawn;
    }
    public void Update()
    {
        if (isRespawning && Time.time >= respawningTime)
        {
            Player.Instance.gameObject.SetActive(true);
            Player.Instance.transform.position = respawnPosition + new Vector2(0, 2f);
            isRespawning = false;
        }
    }
    private void StartRespawn()
    {

        isRespawning = true;
        respawningTime = Time.time + respawningCountDown;
    }

    private void OnDisable()
    {
        stats.Health.OnCurrentValueZero -= StartRespawn;
    }
}