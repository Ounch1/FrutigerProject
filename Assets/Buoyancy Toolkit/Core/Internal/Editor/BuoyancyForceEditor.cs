// Buoyancy Toolkit
// Copyright 2016 Gustav Olsson
using UnityEngine;
using UnityEditor;

namespace BuoyancyToolkit
{
    [CustomEditor(typeof(BuoyancyForce))]
    public class BuoyancyForceEditor : Editor
    {
        public static bool showAdvanced;

        protected SerializedProperty ignoreLayers;

        public void OnEnable()
        {
            ignoreLayers = serializedObject.FindProperty("ignoreLayers");
        }

        public override void OnInspectorGUI()
        {
            BuoyancyForce b = (BuoyancyForce)target;

            // Prepare for changes
            EditorGUI.BeginChangeCheck();
            
            var buoyancyCollider = (Collider)EditorGUILayout.ObjectField("Buoyancy Collider", b.BuoyancyCollider, typeof(Collider), true);
            var quality = (BuoyancyQuality)EditorGUILayout.EnumPopup("Quality", b.Quality);

            var samples = b.Samples;
            if (quality == BuoyancyQuality.Custom)
            {
                samples = EditorGUILayout.IntSlider("Samples", b.Samples, 1, 20);
            }

            var useWeighting = EditorGUILayout.Toggle("Use Weighting", b.UseWeighting);
            var weightFactor = b.WeightFactor;
            if (useWeighting)
            {
                weightFactor = EditorGUILayout.Slider("Weight Factor", b.WeightFactor, 0.0f, 10.0f);
            }

            var dragScalar = EditorGUILayout.Slider("Drag Scalar", b.DragScalar, 0.0f, 10.0f);
            var angularDragScalar = EditorGUILayout.Slider("Angular Drag Scalar", b.AngularDragScalar, 0.0f, 10.0f);

            // Workaround for not being able to display layer masks:
            serializedObject.Update();
            EditorGUILayout.PropertyField(ignoreLayers);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.LabelField("Volume", b.Volume.ToString("F1") + " cubic units");
            EditorGUILayout.LabelField("Submerged Volume", b.SubmergedVolume.ToString("F1") + " cubic units");

            showAdvanced = EditorGUILayout.Foldout(showAdvanced, "Advanced");
            var debugVisualization = b.DebugVisualization;
            if (showAdvanced)
            {
                debugVisualization = EditorGUILayout.Toggle("Debug Visualization", b.DebugVisualization);
            }

            // Handles changes
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(b, "Buoyancy Force Setting");

                b.BuoyancyCollider = buoyancyCollider;
                b.Quality = quality;

                if (quality == BuoyancyQuality.Custom)
                {
                    b.Samples = samples;
                }

                b.UseWeighting = useWeighting;
                b.WeightFactor = weightFactor;
                b.DragScalar = dragScalar;
                b.AngularDragScalar = angularDragScalar;
                b.DebugVisualization = debugVisualization;

                EditorUtility.SetDirty(b);
            }
        }
    }
}