﻿using UnityEngine;
using System.Collections;

namespace uAdventure.Editor
{
    public abstract class EditorWindowExtension : LayoutWindow
    {
        public EditorWindowExtension(Rect rect, params GUILayoutOption[] options) : this(rect, null, null, options) { }
        public EditorWindowExtension(Rect rect, GUIContent content, params GUILayoutOption[] options) : this(rect, content, null, options) { }
        public EditorWindowExtension(Rect rect, GUIStyle style, params GUILayoutOption[] options) : this(rect, null, style, options) { }
        public EditorWindowExtension(Rect rect, GUIContent content, GUIStyle style, params GUILayoutOption[] options) : base(rect, content, style, options) {}

        // Request main view
        public delegate void RequestMainView(EditorWindowExtension extension);
        public RequestMainView OnRequestMainView;

        public virtual bool Selected { get; set; }
        public float ContentHeight { get; set; }
        public abstract void DrawLeftPanelContent(Rect rect, GUIStyle aStyle);
        public abstract void LayoutDrawLeftPanelContent(GUIStyle aStyle, params GUILayoutOption[] aOptions);
    }

}

