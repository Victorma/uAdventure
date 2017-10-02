﻿using UnityEngine;
using UnityEditor;
using System.Collections;

using uAdventure.Core;
using System.Linq;
using System.Collections.Generic;

namespace uAdventure.Editor
{
    [EditorComponent(typeof(ItemDataControl), Name = "Item.ActionsPanelTitle", Order = 10)]
    public class ItemsWindowActions : AbstractEditorComponent
    {
        private Texture2D conditionsTex = null;
        private Texture2D noConditionsTex = null;

        private Vector2 scrollPosition;
        
        private DataControlList actionsList;

        private string documentation = "", documentationLast = "";

        public ItemsWindowActions(Rect aStartPos, GUIContent aContent, GUIStyle aStyle, params GUILayoutOption[] aOptions)
            : base(aStartPos, aContent, aStyle, aOptions)
        {
            conditionsTex = (Texture2D)Resources.Load("EAdventureData/img/icons/conditions-24x24", typeof(Texture2D));
            noConditionsTex = (Texture2D)Resources.Load("EAdventureData/img/icons/no-conditions-24x24", typeof(Texture2D));
            
            actionsList = new DataControlList()
            {
                elementHeight = 40,
                Columns = new System.Collections.Generic.List<ColumnList.Column>()
                {
                    new ColumnList.Column()
                    {
                        Text = TC.get("ActionsList.ActionName"),
                        SizeOptions = new GUILayoutOption[] { GUILayout.Width(150) }
                    },
                    new ColumnList.Column()
                    {
                        Text = TC.get("DescriptionList.Description"),
                        SizeOptions = new GUILayoutOption[] { GUILayout.ExpandWidth(true) }
                    },
                    new ColumnList.Column()
                    {
                        Text = TC.get("ActionsList.NeedsGoTo"),
                        SizeOptions = new GUILayoutOption[] { GUILayout.Width(120) }
                    },
                    new ColumnList.Column()
                    {
                        Text = TC.get("ActionsList.Conditions"),
                        SizeOptions = new GUILayoutOption[] { GUILayout.Width(70) }
                    },
                    new ColumnList.Column()
                    {
                        Text = TC.get("Element.Effects"),
                        SizeOptions = new GUILayoutOption[] { GUILayout.Width(70) }
                    }
                },
                drawCell = (rect, index, column, isActive, isFocused) =>
                {
                    var action = actionsList.list[index] as ActionDataControl;
                    switch (column)
                    {
                        case 0:
                            if (action.hasIdTarget())
                            {
                                var leftHalf = new Rect(rect);
                                leftHalf.width /= 2f;
                                var rightHalf = new Rect(leftHalf);
                                rightHalf.x += leftHalf.width;
                                rightHalf.height = 25;
                                EditorGUI.LabelField(leftHalf, action.getTypeName());
                                if (!isActive)
                                {
                                    EditorGUI.LabelField(rightHalf, !string.IsNullOrEmpty(action.getIdTarget()) ? action.getIdTarget() : "---");
                                }
                                else
                                {
                                    EditorGUI.BeginChangeCheck();
                                    string selected = string.Empty;
                                    List<string> choices = new List<string>();
                                    switch ((action.getContent() as Action).getType())
                                    {
                                        case Action.DRAG_TO:
                                        case Action.CUSTOM_INTERACT:
                                            choices.AddRange(activeAreas);
                                            choices.AddRange(characters);
                                            choices.AddRange(items);
                                            break;
                                        case Action.GIVE_TO:
                                            choices.AddRange(characters);
                                            break;
                                        case Action.USE_WITH:
                                            choices.AddRange(activeAreas);
                                            choices.AddRange(items);
                                            break;
                                    }
                                    var selectedIndex = EditorGUI.Popup(rightHalf, choices.FindIndex(t => t.Equals(action.getIdTarget())), choices.ToArray());
                                    if (EditorGUI.EndChangeCheck())
                                    {
                                        if (selectedIndex >= 0 && selectedIndex < choices.Count)
                                        {
                                            selected = choices[selectedIndex];
                                            action.setIdTarget(selected);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                EditorGUI.LabelField(rect, action.getTypeName());
                            }
                            break;
                        case 1:
                            EditorGUI.BeginChangeCheck();
                            var documentation = EditorGUI.TextArea(rect, action.getDocumentation() ?? string.Empty);
                            if (EditorGUI.EndChangeCheck()) action.setDocumentation(documentation);
                            break;
                        case 2:
                            if (Controller.Instance.playerMode() == Controller.FILE_ADVENTURE_1STPERSON_PLAYER)
                            {
                                EditorGUI.LabelField(rect, TC.get("ActionsList.NotRelevant"));
                            }
                            else
                            {
                                var leftHalf = new Rect(rect);
                                leftHalf.width /= 2f;
                                var rightHalf = new Rect(leftHalf);
                                rightHalf.x += leftHalf.width;

                                EditorGUI.BeginChangeCheck();
                                var needsToGo = EditorGUI.Toggle(leftHalf, action.getNeedsGoTo());
                                if (EditorGUI.EndChangeCheck()) action.setNeedsGoTo(needsToGo);

                                EditorGUI.BeginChangeCheck();
                                var distance = EditorGUI.IntField(rightHalf, action.getKeepDistance());
                                if (EditorGUI.EndChangeCheck()) action.setKeepDistance(distance); ;
                            }
                            break;
                        case 3:
                            if (GUI.Button(rect, action.getConditions().getBlocksCount() > 0 ? conditionsTex : noConditionsTex))
                            {
                                ConditionEditorWindow window = ScriptableObject.CreateInstance<ConditionEditorWindow>();
                                window.Init(action.getConditions());
                            }
                            break;
                        case 4:
                            if (GUI.Button(rect, "Effects"))
                            {
                                EffectEditorWindow window = ScriptableObject.CreateInstance<EffectEditorWindow>();
                                window.Init(action.getEffects());
                            }
                            break;

                    }
                }
            };
        }

        private string[] activeAreas, characters, items;
        public override void Draw(int aID)
        {
            var windowWidth = m_Rect.width;
            var windowHeight = m_Rect.height;

            var workingItem = Target != null ? Target as ItemDataControl : Controller.Instance.SelectedChapterDataControl.getItemsList().getItems()[
                    GameRources.GetInstance().selectedItemIndex];
            if (Target != null) windowHeight = 300;

            activeAreas = Controller.Instance.IdentifierSummary.getIds<ActiveArea>();
            characters  = Controller.Instance.IdentifierSummary.getIds<NPC>();
            items       = Controller.Instance.IdentifierSummary.getIds<Item>();

            actionsList.SetData(workingItem.getActionsList(), (data) => (data as ActionsListDataControl).getActions().Cast<DataControl>().ToList());
            actionsList.DoList(windowHeight - 60f);
        }
    }
}