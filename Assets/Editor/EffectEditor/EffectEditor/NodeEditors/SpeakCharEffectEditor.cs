﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

using uAdventure.Core;

namespace uAdventure.Editor
{
    public class SpeakCharEffectEditor : EffectEditor
    {
        private bool collapsed = false;
        public bool Collapsed { get { return collapsed; } set { collapsed = value; } }
        private Rect window = new Rect(0, 0, 300, 0);
        public Rect Window
        {
            get
            {
                if (collapsed) return new Rect(window.x, window.y, 50, 30);
                else return window;
            }
            set
            {
                if (collapsed) window = new Rect(value.x, value.y, window.width, window.height);
                else window = value;
            }
        }

        private string[] npc;
        private SpeakCharEffect effect;

        public SpeakCharEffectEditor()
        {
            npc = Controller.Instance.SelectedChapterDataControl.getNPCsList().getNPCsIDs();
            this.effect = new SpeakCharEffect(npc.Length > 0 ? npc[0] : "", "");
        }

        public void draw()
        {

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(TC.get("ConversationEditor.Line"));
            effect.setLine(EditorGUILayout.TextField(effect.getLine()));
            EditorGUILayout.LabelField(TC.get("Element.Name28"));
            effect.setTargetId(npc[EditorGUILayout.Popup(Array.IndexOf(npc, effect.getTargetId()), npc)]);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox(TC.get("SpeakCharacterEffect.Description"), MessageType.Info);
        }

        public IEffect Effect { get { return effect; } set { effect = value as SpeakCharEffect; } }
        public string EffectName { get { return TC.get("SpeakCharacterEffect.Title"); } }
        public EffectEditor clone() { return new SpeakPlayerEffectEditor(); }

        public bool manages(IEffect c)
        {

            return c.GetType() == effect.GetType();
        }

        public bool Usable
        {
            get
            {
                return Controller.Instance.SelectedChapterDataControl.getNPCsList().getNPCsIDs().Length > 0;
            }
        }
    }
}