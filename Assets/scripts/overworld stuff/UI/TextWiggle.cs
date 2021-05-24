using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWiggle : MonoBehaviour
{
    public TMP_Text textComponent;
    void Update()
    {
        textComponent.ForceMeshUpdate();
        TMP_TextInfo textInfo = textComponent.textInfo;

        //loop through all the letters in the component right now
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            //the letters info
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
            {
                continue;
            }

            //all the vertices for this current letter
            Vector3[] verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                Vector3 orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.1f) * 1f, 0);
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
