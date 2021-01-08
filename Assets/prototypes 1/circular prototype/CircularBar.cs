using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularBar : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 start, end;
    bool activated;

    private void Start()
    {
        end = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        activated = CircularBattleManager.current.battleStarted;

        // if (activated)
        // {
        //     if (LightweightTrackTimeManager.current.songPositionInBeats / start.x < 1)
        //     {
        //         transform.localScale = Vector3.Lerp(start, end, LightweightTrackTimeManager.current.songPositionInBeats / start.x);

        //     }
        //     else
        //     {
        //         Destroy(this.gameObject);
        //     }

        // }
    }
}
