using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraTrackSwitcher : MonoBehaviour
{

    CinemachineDollyCart cart;

    CinemachineVirtualCamera cam;

    public CinemachineSmoothPath startPath;
    public CinemachineSmoothPath[] alternatePaths;

    void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        cart = GetComponent<CinemachineDollyCart>();
        Reset();

    }

    void Reset()
    {
        StopAllCoroutines();
        cart.m_Path = startPath;
        //StartCoroutine(ChangeTrack());

    }


    //rather than having this randomly timed on a recursive loop have this be a 50% chance to happen every bar
    IEnumerator ChangeTrack()
    {
        yield return new WaitForSeconds(Random.Range(4, 6));

        var path = alternatePaths[Random.Range(0, alternatePaths.Length)];
        cart.m_Path = path;

        StartCoroutine(ChangeTrack());
    }



    public void NextCamTrack()
    {
        CinemachineSmoothPath path;
        path = alternatePaths[Random.Range(0, alternatePaths.Length)];

        //so we dont pick the same path
        while (path == cart.m_Path)
        {
            path = alternatePaths[Random.Range(0, alternatePaths.Length)];
        }

        cart.m_Path = path;
    }


}
