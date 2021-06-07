using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NBattleUIManager : MonoBehaviour
{
    public Transform turnQueuePanel;
    public GameObject turnInfoPrefab;

    public static NBattleUIManager current;

    public List<Transform> turnOrderQueue;

    //ENEMY PROTOTYPE SHIT


    private void Awake()
    {
        current = this;

    }

    public void InitTurnQueue(List<BattleAction> turnQueue)
    {


        turnOrderQueue = new List<Transform>();
        for (int i = 0; i < turnQueue.Count; i++)
        {
            GameObject turnInfo = Instantiate(turnInfoPrefab);
            turnInfo.transform.SetParent(turnQueuePanel);
            turnInfo.transform.position = new Vector3(Screen.width / 2 + (i * 200), 75, 0);

            if (turnQueue[i].playerOrEnemy == true)
            {
                turnInfo.GetComponent<TurnInfo>().SetInfo(((PlayerBattleAction)turnQueue[i]).sample.sampleName, ((PlayerBattleAction)turnQueue[i]).sample.numericValue.ToString());
            }
            else
            {
                turnInfo.GetComponent<TurnInfo>().SetInfo("Enemy Attack", ((EnemyBattleAction)turnQueue[i]).dmg.ToString());

            }

            if (turnQueue[i].playerOrEnemy)
            {
                turnInfo.GetComponent<Image>().color = Color.green;
            }
            else
            {
                turnInfo.GetComponent<Image>().color = Color.red;
            }

            turnOrderQueue.Add(turnInfo.transform);

        }
    }

    public void UpdateTurnQueue()
    {
        Debug.Log("shift everything over");
        //could just redraw this but naw we're gonna lerp it later so for now just move everything over one position

        //shift all the positions over by one
        for (int i = turnOrderQueue.Count - 1; i > 0; i--)
        {
            turnOrderQueue[i].transform.position = turnOrderQueue[i - 1].transform.position;
        }

        Destroy(turnOrderQueue[0].gameObject);
        turnOrderQueue.RemoveAt(0);
    }


}
