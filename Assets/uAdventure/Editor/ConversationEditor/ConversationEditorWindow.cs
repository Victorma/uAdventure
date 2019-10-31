﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using uAdventure.Core;
using System;
using System.Linq;

namespace uAdventure.Editor
{
    public class ConversationEditor : CollapsibleGraphEditor<ConversationDataControl, ConversationNodeDataControl>
    {
        private bool elegibleForClick = false;
        private readonly Dictionary<ConversationNodeDataControl, ConversationNodeEditor> editors = new Dictionary<ConversationNodeDataControl, ConversationNodeEditor>();

        protected override ConversationNodeDataControl[] ChildsFor(ConversationDataControl Content, ConversationNodeDataControl parent)
        {
            return parent.getChilds().ToArray();
        }
        protected override void OnAfterDrawWindows()
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    elegibleForClick = true;
                    break;
                case EventType.MouseUp:
                    if(elegibleForClick && lookingChildNode != null)
                    {
                        var options = new List<GUIContent> { new GUIContent("Cancel asignation") };
                        foreach(var addable in lookingChildNode.getAddableNodes())
                        {
                            options.Add(new GUIContent("Create/" + TC.get("Element.Name" + addable)));
                        }

                        EditorUtility.DisplayCustomMenu(new Rect(Event.current.mousePosition, Vector2.one), options.ToArray(), -1, (param, ops, selected) =>
                        {
                            if(selected != 0)
                            {
                                var option = lookingChildNode.getAddableNodes()[selected - 1];
                                Content.linkNode(lookingChildNode, option, lookingChildSlot);
                            }
                            lookingChildNode = null;
                            lookingChildSlot = -1;
                        }, Event.current.mousePosition);
                        elegibleForClick = false;
                    }
                    break;
                case EventType.MouseDrag:
                    elegibleForClick = false;
                    break;
            }

            base.OnAfterDrawWindows();
        }

        protected override void DeleteNode(ConversationDataControl content, ConversationNodeDataControl node)
        {
            if (content.deleteNode(node) && Selection.Contains(node))
            {
                Selection.Remove(node);
            }
        }

        protected override void DrawOpenNodeContent(ConversationDataControl content, ConversationNodeDataControl node)
        {
            ConversationNodeEditor editor = null;
            if(editors.TryGetValue(node, out editor))
            {
                editor.draw();
            }
        }

        protected override void DrawNodeControls(ConversationDataControl content, ConversationNodeDataControl node)
        {
            ConversationNodeEditor editor = null;
            editors.TryGetValue(node, out editor);

            string[] editorNames = ConversationNodeEditorFactory.Intance.CurrentConversationNodeEditors;
            int preEditorSelected = ConversationNodeEditorFactory.Intance.ConversationNodeEditorIndex(node);
            int editorSelected = EditorGUILayout.Popup(preEditorSelected, editorNames);

            if (editor == null || preEditorSelected != editorSelected)
            {
                bool firstEditor = (editor == null);
                var intRect = node.getEditorRect();
                var prevRect = (editor != null) ? editor.Window : new Rect(intRect.x, intRect.y, intRect.width, intRect.height);
                editor = ConversationNodeEditorFactory.Intance.createConversationNodeEditorFor(content, editorNames[editorSelected]);
                editor.setParent(this);
                editor.Window = prevRect;
                if (firstEditor) editor.Node = node;
                else setNode(content, node, editor.Node);

                editors.Remove(node);
                editors[editor.Node] = editor;

                if (Selection.Contains(node))
                {
                    Selection.Remove(node);
                    Selection.Add(editor.Node);
                }
            }

            base.DrawNodeControls(content, node);
        }

        private bool setNode(ConversationDataControl content, ConversationNodeDataControl oldNode, ConversationNodeDataControl newNode)
        {
            return Content.replaceNode(oldNode, newNode);
        }      

        protected override ConversationNodeDataControl[] GetNodes(ConversationDataControl Content)
        {
            return Content.getAllNodes().ToArray();
        }

        protected override string GetTitle(ConversationDataControl Content, ConversationNodeDataControl node)
        {
            return node.ToString();
        }

        protected override void SetNodeChild(ConversationDataControl content, ConversationNodeDataControl node, int slot, ConversationNodeDataControl child)
        {
            content.linkNode(node, child, slot);
        }

        protected override void SetCollapsed(ConversationDataControl Content, ConversationNodeDataControl node, bool collapsed)
        {
            node.setEditorCollapsed(collapsed);
        }

        protected override bool IsCollapsed(ConversationDataControl Content, ConversationNodeDataControl node)
        {
            return node.getEditorCollapsed();
        }

        protected override Rect GetOpenedNodeRect(ConversationDataControl content, ConversationNodeDataControl node)
        {
            var intRect = node.getEditorRect();
            var rect = new Rect(intRect.x, intRect.y, intRect.width, intRect.height);
            return editors.ContainsKey(node) ? editors[node].Window : rect;
        }

        protected override void SetNodePosition(ConversationDataControl content, ConversationNodeDataControl node, Vector2 position)
        {
            var nodeRect = node.getEditorRect();
            nodeRect.position = new Vector2Int((int)position.x, (int)position.y);
            node.setEditorRect(nodeRect);

            if (editors.ContainsKey(node))
            {
                var rect = editors[node].Window;
                rect.position = position;
                editors[node].Window = rect;
            }
        }

        protected override void SetNodeSize(ConversationDataControl content, ConversationNodeDataControl node, Vector2 size)
        {
            var nodeRect = node.getEditorRect();
            nodeRect.size = new Vector2Int((int)size.x, (int)size.y);
            node.setEditorRect(nodeRect);

            if (editors.ContainsKey(node))
            {
                var rect = editors[node].Window;
                rect.size = size;
                editors[node].Window = rect;
            }
        }

        protected override void MoveNodes(ConversationDataControl Content, IEnumerable<ConversationNodeDataControl> nodes, Vector2 delta)
        {
            Content.moveNodes(nodes.First(), nodes.ToList(), new Vector2Int(Mathf.RoundToInt(delta.x), Mathf.RoundToInt(delta.y)));
        }

        protected override void OnDrawLine(ConversationDataControl content, ConversationNodeDataControl originNode, ConversationNodeDataControl destinationNode, Rect originRect, Rect destinationRect, bool isHovered, bool isRemoving)
        {
            var addButton = new Rect(0, 0, 20, 20);
            addButton.center = (originRect.center + destinationRect.center) / 2f;

            Debug.Log(Event.current.type);
            if(GUI.Button(addButton, "+"))
            {
                var options = new List<GUIContent>();
                foreach (var addable in originNode.getAddableNodes())
                {
                    options.Add(new GUIContent("Create " + TC.get("Element.Name" + addable)));
                }

                EditorUtility.DisplayCustomMenu(new Rect(Event.current.mousePosition, Vector2.one), options.ToArray(), -1, (param, ops, selected) =>
                {
                    int index = originNode.getChilds().IndexOf(destinationNode);
                    var option = originNode.getAddableNodes()[selected];
                    Content.insertNode(originNode, option, index);
                }, Event.current.mousePosition);
            }
        }
    }

    public class ConversationEditorWindow : EditorWindow
    {
        /*******************
		 *  PROPERTIES
		 * *****************/
        private ConversationEditor conversationEditor;
        public ConversationDataControl Conversation { get; set; }
        

        /*******************************
		 * Initialization methods
		 ******************************/
        public void Init(ConversationDataControl conversation)
        {
            Conversation = conversation;

            ConversationNodeEditorFactory.Intance.ResetInstance();
            conversationEditor = CreateInstance<ConversationEditor>();
            conversationEditor.BeginWindows = BeginWindows; 
            conversationEditor.EndWindows = EndWindows;
            conversationEditor.Repaint = Repaint;
            conversationEditor.Init(conversation);
        }

        

        /******************
		 * Window behaviours
		 * ******************/

        protected void OnGUI()
        {
            if (Conversation == null)
            {
                this.Close();
            }
            
            this.wantsMouseMove = true;
            
            GUILayout.BeginVertical(GUILayout.Height(20));
            Conversation.setId(EditorGUILayout.TextField(TC.get("Conversation.Title"), Conversation.getId(),
                GUILayout.ExpandWidth(true)));
            GUILayout.EndVertical();

            conversationEditor.OnInspectorGUI();
        }
    }
}