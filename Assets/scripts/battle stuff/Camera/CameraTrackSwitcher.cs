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
        StartCoroutine(ChangeTrack());
        
    }

    IEnumerator ChangeTrack()
    {
        yield return new WaitForSeconds(Random.Range(4, 6));

        var path = alternatePaths[Random.Range(0, alternatePaths.Length)];
        cart.m_Path = path;

        StartCoroutine(ChangeTrack());
    }


}
