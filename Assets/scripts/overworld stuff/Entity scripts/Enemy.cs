using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Enemy : MonoBehaviour
{
    public int health, maxHealth;
    Collider col;
    public string enemyName;
    public Track[] battleTracks;

    // Start is called before the first frame update
    void Start()
    {
        //add ourself to the scene managers dictionary of enemies so we can be activated/deactivated on scene loads
        //SceneManage.current.enemySceneDictionary.Add(this, SceneManager.GetActiveScene().name);

        if (SceneManage.current != null && SceneManager.GetActiveScene().name != "BattleScene")
        {
            SceneManage.current.enemies.Add(this);
        }

        col = GetComponent<Collider>();

        foreach (Track track in battleTracks)
        {
            //print(Array.ConvertAll(track.kickBeats.indicatorData.Split(','), float.Parse));
            track.kickBeats.indicatorPositions = Array.ConvertAll(track.kickBeats.indicatorData.Split(' '), float.Parse);
            track.snareBeats.indicatorPositions = Array.ConvertAll(track.snareBeats.indicatorData.Split(' '), float.Parse);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {


            if (!SceneManage.current.inBattle)
            {
                SceneManage cur = SceneManage.current;
                SceneManage.current.TransitionToBattle(this.gameObject, battleTracks[UnityEngine.Random.Range(0, battleTracks.Length)]);
            }
        }
    }

    private void OnDestroy()
    {
        //check if this is a testing mode
        if (SceneManage.current != null)
        {
            SceneManage.current.enemies.Remove(this);
        }
    }
}
