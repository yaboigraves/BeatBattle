using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCrowdManager : MonoBehaviour
{
    public GameObject[] audienceMembers;
    public int numAudienceMembers;
    public Transform audienceSpawnOrigin;

    void Start()
    {
        LoadAudience();
    }

    void LoadAudience()
    {
        for (int i = 0; i < numAudienceMembers; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-10, 10), 0, Random.Range(-5, 5));
            GameObject audienceMember = Instantiate(audienceMembers[Random.Range(0, audienceMembers.Length)], audienceSpawnOrigin.transform.position + offset, Quaternion.identity);
        }
    }
}
