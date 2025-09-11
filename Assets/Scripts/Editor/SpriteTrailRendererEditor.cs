using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SpriteTrailRenderer
{
    [CustomEditor(typeof(SpriteTrailRenderer))]
    public class SpriteTrailRendererEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            serializedObject.Update();

            SpriteTrailRenderer trailRenderer = (SpriteTrailRenderer)target;

            // update trail spawn values
            trailRenderer._trailLifetime = EditorGUILayout.FloatField("Trail Lifetime", trailRenderer._trailLifetime);
            trailRenderer._spawnType = (SpawnType)EditorGUILayout.EnumPopup("Trail Spawn Type", trailRenderer._spawnType);

            if (trailRenderer._spawnType == SpawnType.TIME)
            {
                trailRenderer._timeBetweenTrailSpawn = EditorGUILayout.FloatField("Spawn Rate", trailRenderer._timeBetweenTrailSpawn);
            }
            else
            {
                trailRenderer._distanceBetweenTrailSpawn = EditorGUILayout.FloatField("Distance Between Spawns", trailRenderer._distanceBetweenTrailSpawn);
            }

            // update trail scales
            trailRenderer._startScale = EditorGUILayout.Vector2Field("Start Scale", trailRenderer._startScale);
            trailRenderer._endScale = EditorGUILayout.Vector2Field("End Scale", trailRenderer._endScale);

            // update color alpha
            trailRenderer._alphaUpdateOn = EditorGUILayout.Toggle("Alpha Update On", trailRenderer._alphaUpdateOn);
            trailRenderer._maxAlpha = EditorGUILayout.DelayedFloatField("Max Alpha", trailRenderer._maxAlpha);
            trailRenderer._minAlpha = EditorGUILayout.DelayedFloatField("Min Alpha", trailRenderer._minAlpha);

            if (trailRenderer._minAlpha > trailRenderer._maxAlpha)
            {
                trailRenderer._minAlpha = trailRenderer._maxAlpha;
            }


            // color options
            trailRenderer._useSolidColors = EditorGUILayout.Toggle("Change Trail Colors", trailRenderer._useSolidColors);
            if (trailRenderer._useSolidColors)
            {
                trailRenderer._rainbowMode = EditorGUILayout.Toggle("Rainbow Mode", trailRenderer._rainbowMode);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_userSelectedColorPalette"), true);
            }

            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }
    }
}