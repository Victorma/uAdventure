﻿using UnityEngine;
using System.Collections;

using uAdventure.Core;
using UnityEditor;
using System.Collections.Generic;

namespace uAdventure.Editor
{
    public class ItemsWindowAppearance : LayoutWindow
    {

        private Texture2D imageTex = null;
        private Texture2D iconTex = null;
        private Texture2D imageOverTex = null;

        private Texture2D conditionsTex = null;
        private Texture2D noConditionsTex = null;

        private string imagePath = "";
        private string inventoryIconPath = "";
        private string imageWhenOverPath = "";

        private FileChooser image, icon, image_over;

        private ItemDataControl workingItem;
        private AppearanceEditor appearanceEditor;

        public ItemsWindowAppearance(Rect aStartPos, GUIContent aContent, GUIStyle aStyle, params GUILayoutOption[] aOptions)
            : base(aStartPos, aContent, aStyle, aOptions)
        {

            conditionsTex = (Texture2D)Resources.Load("EAdventureData/img/icons/conditions-24x24", typeof(Texture2D));
            noConditionsTex = (Texture2D)Resources.Load("EAdventureData/img/icons/no-conditions-24x24", typeof(Texture2D));

            appearanceEditor = ScriptableObject.CreateInstance<AppearanceEditor>();
            appearanceEditor.height = 160;
            appearanceEditor.onAppearanceSelected = RefreshPathInformation;
            // File selectors

            image = new FileChooser()
            {
                Label = TC.get("Resources.DescriptionItemImage"),
                FileType = BaseFileOpenDialog.FileType.ITEM_IMAGE
            };

            icon = new FileChooser()
            {
                Label = TC.get("Resources.DescriptionItemIcon"),
                FileType = BaseFileOpenDialog.FileType.ITEM_ICON
            };

            image_over = new FileChooser()
            {
                Label = TC.get("Resources.DescriptionItemImageOver"),
                FileType = BaseFileOpenDialog.FileType.ITEM_IMAGE_OVER
            };
        }

        
        public override void Draw(int aID)
        {
            var previousWorkingItem = workingItem;
            workingItem = Controller.Instance.SelectedChapterDataControl.getItemsList().getItems()[GameRources.GetInstance().selectedItemIndex];

            // Appearance table
            appearanceEditor.Data = workingItem;
            appearanceEditor.OnInspectorGUI();

            GUILayout.Space(10);

            EditorGUI.BeginChangeCheck();

            string previousValue = image.Path = workingItem.getPreviewImage();
            image.DoLayout(GUILayout.ExpandWidth(true));
            if(previousValue != image.Path) workingItem.setPreviewImage(image.Path);

            previousValue = icon.Path = workingItem.getIconImage();
            icon.DoLayout(GUILayout.ExpandWidth(true));
            if (previousValue != icon.Path) workingItem.setIconImage(icon.Path);

            previousValue = image_over.Path = workingItem.getMouseOverImage();
            image_over.DoLayout(GUILayout.ExpandWidth(true));
            if (previousValue != image_over.Path) workingItem.setMouseOverImage(image_over.Path);

            if (EditorGUI.EndChangeCheck())
            {
                RefreshPathInformation(workingItem);
            }

            GUILayout.Space(10);

            GUILayout.Label(TC.get("ImageAssets.Preview"), "preToolbar", GUILayout.ExpandWidth(true));
            var rect = EditorGUILayout.BeginVertical("preBackground", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                GUILayout.BeginHorizontal();
                {
                    rect.x += 30;
                    rect.width -= 60;
                    rect.y += 30;
                    rect.height -= 60;

                    GUI.DrawTexture(rect, rect.Contains(Event.current.mousePosition) && imageOverTex ? imageOverTex : imageTex, ScaleMode.ScaleToFit);
                }
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        private void RefreshPathInformation(DataControlWithResources dataControl)
        {
            var item = dataControl as ItemDataControl;

            imagePath           = item.getPreviewImage();
            inventoryIconPath   = item.getIconImage();
            imageWhenOverPath   = item.getMouseOverImage();

            imageTex        = string.IsNullOrEmpty(imagePath) ? null : AssetsController.getImage(imagePath).texture;
            iconTex         = string.IsNullOrEmpty(inventoryIconPath) ? null :AssetsController.getImage(inventoryIconPath).texture;
            imageOverTex    = string.IsNullOrEmpty(imageWhenOverPath) ? null : AssetsController.getImage(imageWhenOverPath).texture;
        }
    }
}