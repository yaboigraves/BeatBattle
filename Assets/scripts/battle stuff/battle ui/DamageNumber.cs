using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage;
    public float remainTime;
    public TextMeshProUGUI text;
    void Start()
    {
        LeanTween.move(this.gameObject, transform.position + (Vector3.up * 100), remainTime).setDestroyOnComplete(true);
    }

    IEnumerator dieRoutine()
    {
        yield return new WaitForSeconds(remainTime);
        Destroy(this.gameObject);
    }

    public void setDamage(int dmg)
    {
        text.text = dmg.ToString();
    }
}
