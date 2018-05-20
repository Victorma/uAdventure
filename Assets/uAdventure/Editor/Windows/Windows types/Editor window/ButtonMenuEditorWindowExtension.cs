﻿using UnityEngine;
using System.Collections;
using System;
using UnityEditor.AnimatedValues;
using UnityEditor;

namespace uAdventure.Editor
{
    public abstract class ButtonMenuEditorWindowExtension : EditorWindowExtension
    {
        protected AnimBool extended;
        public GUIContent ButtonContent { get; set; }
        protected bool UseAnimation { get; set; }

        public override bool Selected
        {
            get
            {
                return base.Selected;
            }

            set
            {
                base.Selected = value;
                extended.target = Selected;
            }
        }

        public ButtonMenuEditorWindowExtension(Rect rect, params GUILayoutOption[] aOptions) : this(rect, null, null, aOptions) { }
        public ButtonMenuEditorWindowExtension(Rect rect, GUIStyle style, params GUILayoutOption[] aOptions) : this(rect, null, style, aOptions) { }
        public ButtonMenuEditorWindowExtension(Rect rect, GUIContent content, params GUILayoutOption[] aOptions) : this(rect, content, null, aOptions) { }
        public ButtonMenuEditorWindowExtension(Rect rect, GUIContent content, GUIStyle style, params GUILayoutOption[] aOptions) : base(rect, content, style, aOptions)
        {
            UseAnimation = true;
            extended = new AnimBool(false);
        }

        public override void DrawLeftPanelContent(Rect rect, GUIStyle aStyle)
        {
            var bv = new Vector2(ButtonHeight, 0);
            var buttonRect = new Rect(rect.position, bv + new Vector2(0, rect.width));
            var menuRect = new Rect(rect.position + bv, rect.size - bv);

            if(DrawButton(buttonRect, aStyle))
                OnButton();
            DrawMenu(menuRect, aStyle);
        }

        public override void LayoutDrawLeftPanelContent(GUIStyle aStyle, params GUILayoutOption[] aOptions)
        {
            if (LayoutDrawButton(aStyle, aOptions))
                OnButton();

            if (!UseAnimation || EditorGUILayout.BeginFadeGroup(extended.faded))
                LayoutDrawMenu(aStyle, aOptions);

            if (UseAnimation)
            {
                EditorGUILayout.EndFadeGroup();
                OnRequestRepaint();
            }
        }

        void UpdateTotalHeight()
        {
            ContentHeight = buttonHeight + menuHeight;
        }

        // Button part
        protected float buttonHeight;
        public float ButtonHeight { get { return buttonHeight; } set { buttonHeight = value; UpdateTotalHeight(); } }
        public abstract bool DrawButton(Rect rect, GUIStyle aStyle);
        public abstract bool LayoutDrawButton(GUIStyle aStyle, params GUILayoutOption[] aOptions);
        public virtual bool LayoutDrawButton() { return LayoutDrawButton("Button"); }

        // Menu part
        protected float menuHeight;
        public float MenuHeight { get { return menuHeight; } set { menuHeight = value; UpdateTotalHeight(); } }
        public abstract void DrawMenu(Rect rect, GUIStyle aStyle);
        public abstract void LayoutDrawMenu(GUIStyle aStyle, params GUILayoutOption[] aOptions);
        public virtual void LayoutDrawMenu() { LayoutDrawMenu(null); }

        // OnButton
        protected abstract void OnButton();
    }
}

