﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Lofelt.NiceVibrations
{
    [CustomEditor(typeof(HapticSource))]
    [CanEditMultipleObjects]
    /// <summary>
    /// Provides an inspector for the <c>HapticSource</c> component.
    /// </summary>
    ///
    /// The inspector lets you link a <c>HapticSource</c> to a <c>HapticClip</c>.
    public class HapticSourceInspector : Editor
    {
        string hapticsDirectory;

        SerializedProperty hapticClip;
        SerializedProperty priority;
        SerializedProperty amplitudeMultiplication;
        SerializedProperty frequencyShift;
        SerializedProperty loop;

        public static GUIContent hapticClipLabel = EditorGUIUtility.TrTextContent("Haptic Clip", "The HapticClip asset played by the HapticSource.");
        public static GUIContent loopLabel = EditorGUIUtility.TrTextContent("Loop", "Set the haptic source to loop playback of the haptic clip");

        void OnEnable()
        {
            hapticClip = serializedObject.FindProperty("clip");
            priority = serializedObject.FindProperty("priority");
            amplitudeMultiplication = serializedObject.FindProperty("_amplitudeMultiplication");
            frequencyShift = serializedObject.FindProperty("_frequencyShift");
            loop = serializedObject.FindProperty("_loop");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(hapticClip, hapticClipLabel);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(loop, loopLabel);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            CreatePrioritySlider();
            CreateAmplitudeMultiplicationSlider();
            CreateFrequencyShiftSlider();

            serializedObject.ApplyModifiedProperties();
        }

        /// Helper function to create a priority slider for haptic source with High and Max text labels.
        void CreatePrioritySlider()
        {
            Rect position = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);

            EditorGUI.IntSlider(position, priority, 0, 256);

            // Move to next line
            position.y += EditorGUIUtility.singleLineHeight;

            // Subtract the label
            position.x += EditorGUIUtility.labelWidth;
            position.width -= EditorGUIUtility.labelWidth;

            // Subtract the text field width thats drawn with slider
            position.width -= EditorGUIUtility.fieldWidth;

            GUIStyle style = GUI.skin.label;
            TextAnchor defaultAlignment = GUI.skin.label.alignment;
            style.alignment = TextAnchor.UpperLeft; EditorGUI.LabelField(position, "High", style);
            style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position, "Low", style);
            GUI.skin.label.alignment = defaultAlignment;

            // Allow space for the High/Low labels
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        /// Helper function to create a amplitude multiplication slider for haptic
        /// source with labels.
        void CreateAmplitudeMultiplicationSlider()
        {
            Rect position = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);

            EditorGUI.Slider(position, amplitudeMultiplication, 0.0f, 5.0f);

            // Move to next line
            position.y += EditorGUIUtility.singleLineHeight;

            // Subtract the label
            position.x += EditorGUIUtility.labelWidth;
            position.width -= EditorGUIUtility.labelWidth;

            // Subtract the text field width thats drawn with slider
            position.width -= EditorGUIUtility.fieldWidth;

            GUIStyle style = GUI.skin.label;
            TextAnchor defaultAlignment = GUI.skin.label.alignment;
            style.alignment = TextAnchor.UpperLeft; EditorGUI.LabelField(position, "0.0", style);
            style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position, "5.0", style);
            GUI.skin.label.alignment = defaultAlignment;

            // Allow space for the labels
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        /// Helper function to create a frequency shift slider for haptic
        /// source with labels.
        void CreateFrequencyShiftSlider()
        {
            Rect position = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);

            EditorGUI.Slider(position, frequencyShift, -1.0f, 1.0f);

            // Move to next line
            position.y += EditorGUIUtility.singleLineHeight;

            // Subtract the label
            position.x += EditorGUIUtility.labelWidth;
            position.width -= EditorGUIUtility.labelWidth;

            // Subtract the text field width thats drawn with slider
            position.width -= EditorGUIUtility.fieldWidth;

            GUIStyle style = GUI.skin.label;
            TextAnchor defaultAlignment = GUI.skin.label.alignment;
            style.alignment = TextAnchor.UpperLeft; EditorGUI.LabelField(position, "-1.0", style);
            style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position, "1.0", style);
            GUI.skin.label.alignment = defaultAlignment;

            // Allow space for the labels
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
    }
}
