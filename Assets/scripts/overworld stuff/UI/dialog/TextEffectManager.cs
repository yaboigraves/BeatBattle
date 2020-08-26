using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;




public class TextEffectManager : MonoBehaviour
{
    public TMP_Text textComponent;
    public bool effectsEnabled;
    List<(string, (int, int))> currentEffects = new List<(string, (int, int))>();
    List<(int, int)> wiggleRanges = new List<(int, int)>();

    public UIManager uIManager;

    private void Start()
    {
        uIManager = GetComponent<UIManager>();
    }

    //so every time a new line of dialogue is rendered this needs to be updated
    //every line of dialogue we take in a list of effects and their indexes (string,(int,int))
    //every update we go through this list (if it exists) and add these indexes to their respective effect functions

    //effects is a list of nested tuples where the string is the effect name and the ints are the positions 
    //ex : ("wiggle",(2,5))
    public void UpdateTextEffects(List<(string, (int, int))> newEffects)
    {
        currentEffects = newEffects;
        //loop through the current effects and add each range to its appropriate list 

        for (int i = 0; i < newEffects.Count; i++)
        {
            switch (newEffects[i].Item1)
            {
                case "wiggle":
                    wiggleRanges.Add((newEffects[i].Item2.Item1, newEffects[i].Item2.Item2));
                    break;
            }
        }
    }

    public void toggleEffects(bool enabled)
    {
        effectsEnabled = enabled;
    }

    public void ClearEffects()
    {
        currentEffects.Clear();
        wiggleRanges.Clear();
    }

    void WiggleEffect()
    {
        TMP_TextInfo textInfo = textComponent.textInfo;
        //loop through the ranges that need the wiggle effect applied and apply it
        for (int i = 0; i < wiggleRanges.Count; i++)
        {
            TMP_CharacterInfo charInfo;

            for (int x = wiggleRanges[i].Item1; x < wiggleRanges[i].Item2; x++)
            {

                if (x >= uIManager.currentLetter)
                {
                    break;
                }

                //the letters inf
                if (textInfo.characterInfo.Length <= x)
                {
                    Debug.LogError("out of bounds problem with text effect at position" + x);
                    charInfo = textInfo.characterInfo[0];
                }
                else
                {
                    charInfo = textInfo.characterInfo[x];
                }


                if (!charInfo.isVisible)
                {
                    continue;
                }

                //all the vertices for this current letter
                Vector3[] verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

                for (int j = 0; j < 4; j++)
                {
                    Vector3 orig = verts[charInfo.vertexIndex + j];
                    verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.1f) * 3f, 0);
                }
            }
        }
    }

    private void LateUpdate()
    {
        //TODO: need to make it so that this update function can only loop through the text that HAS been rendered already

        if (effectsEnabled)
        {
            //apply any text effects that need applying
            textComponent.ForceMeshUpdate();
            TMP_TextInfo textInfo = textComponent.textInfo;

            WiggleEffect();

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                textComponent.UpdateGeometry(meshInfo.mesh, i);
            }
        }

    }
}