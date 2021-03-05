using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Cards.Editor
{
    [CustomEditor(typeof(CardType))]
    public class CardTypeEditor : UnityEditor.Editor
    {
        private CardType _cardType;
        
        private SerializedProperty _description;
        private SerializedProperty _layers;

        private ReorderableList _layerList;
        private ReorderableList _contentList;
        
        private static GUIStyle _strongStyle;
        private static GUIStyle _layerListStyle;

        private string[] layerNames;

        private void OnEnable()
        {
            _cardType = target as CardType;
            
            
            _description = serializedObject.FindProperty("description");
            _layers = serializedObject.FindProperty("layers");

            RecalculateLayerNames();
            
            CreateLayerList();
            _contentList = null;
            
            _strongStyle = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                normal =
                {
                    textColor = Color.white
                }
            };
        }

        private void RecalculateLayerNames()
        {
            layerNames = new string[_layers.arraySize];
            for (int i = 0; i < layerNames.Length; i++)
                layerNames[i] = _layers.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue;
        }

        public override void OnInspectorGUI()
        {
            _layerListStyle = new GUIStyle(GUI.skin.textField)
            {
                alignment = TextAnchor.MiddleCenter
            };
            
            EditorStyles.textField.wordWrap = true;
            
            // DESCRIPTION FIELD
            EditorGUILayout.LabelField("Description", _strongStyle);
            _description.stringValue = EditorGUILayout.TextArea(_description.stringValue);

            GUILayout.Space(10f);
            
            // LAYERS FIELD
            _layerList.DoLayoutList();
            
            // CONTENTS LIST
            if (_layerList.index >= 0)
            {
                if (_contentList is null)
                    CreateContentList();
                else
                    _contentList.serializedProperty = _layers.GetArrayElementAtIndex(_layerList.index)
                        .FindPropertyRelative("contents");
                
                _contentList.DoLayoutList();
            }
            
            serializedObject.ApplyModifiedProperties();
        }


        private void CreateLayerList()
        {
            _layerList = new ReorderableList(serializedObject, _layers)
            {
                displayAdd = true,
                displayRemove = true
            };

            _layerList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Layers", _strongStyle);
            };

            _layerList.drawElementCallback = (rect, index, active, focused) =>
            {
                SerializedProperty element = _layers.GetArrayElementAtIndex(index);
                SerializedProperty elementName = element.FindPropertyRelative("name");

                elementName.stringValue =
                    EditorGUI.TextField(
                        new Rect(
                            rect.x + 10,
                            rect.y + (rect.height - EditorGUIUtility.singleLineHeight) / 2f,
                            rect.width * .9f,
                            EditorGUIUtility.singleLineHeight
                            ),
                        elementName.stringValue,
                        _layerListStyle
                        );
            };

            _layerList.elementHeightCallback += index =>
            {
                float propertyHeight =
                    EditorGUI.GetPropertyHeight(_layers.GetArrayElementAtIndex(index), false);

                float spacing = EditorGUIUtility.singleLineHeight / 2;
            
                return propertyHeight + spacing;
            };
            
            _layerList.onAddCallback += list =>
            {
                list.index = list.count;
                _cardType.AddLayer();
                serializedObject.Update();
                RecalculateLayerNames();
            };
            
            _layerList.onRemoveCallback += list =>
            {
                _cardType.RemoveLayer(list.index);

                if (list.index >= list.count - 1)
                    list.index--;
                
                serializedObject.Update();
                RecalculateLayerNames();
            };
        }

        private void CreateContentList()
        {
            SerializedProperty contents =
                _layers.GetArrayElementAtIndex(_layerList.index).FindPropertyRelative("contents");
            
            _contentList = new ReorderableList(serializedObject, contents)
            {
                displayAdd = true,
                displayRemove = true
            };
            
            _contentList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, $"{_layers.GetArrayElementAtIndex(_layerList.index).FindPropertyRelative("name").stringValue} Contents", _strongStyle);
            };

            _contentList.drawElementCallback = (rect, index, active, focused) =>
            {
                SerializedProperty content = _contentList.serializedProperty.GetArrayElementAtIndex(index);
                SerializedProperty material = content.FindPropertyRelative("material");
                SerializedProperty contentName = content.FindPropertyRelative("name");

                Rect controlRect = new Rect(
                    rect.x + 10,
                    rect.y,
                    rect.width * .9f,
                    EditorGUIUtility.singleLineHeight
                    );
                
                EditorGUI.LabelField(controlRect, contentName.stringValue, _strongStyle);

                if (!active || material.objectReferenceValue is null)
                    return;
                
                UnityEditor.Editor materialEditor = CreateEditor(material.objectReferenceValue);

                controlRect = EditorGUILayout.GetControlRect();
                controlRect.height = (Screen.height - controlRect.y) * .9f;
                    
                materialEditor.DrawDefaultInspector();
                
                DestroyImmediate(materialEditor);
            };

            _contentList.elementHeightCallback += index =>
            {
                SerializedProperty content = _contentList.serializedProperty.GetArrayElementAtIndex(index);
                float propertyHeight =
                    EditorGUI.GetPropertyHeight(content, false);

                float spacing = EditorGUIUtility.singleLineHeight / 2;
            
                return propertyHeight + spacing;
            };

            _contentList.onAddCallback += list =>
            {
                list.index = list.count;
                _cardType.AddContent(_layerList.index);
                serializedObject.Update();
            };

            _contentList.onRemoveCallback += list =>
            {
                _cardType.RemoveContent(_layerList.index, list.index);

                if (list.index >= list.count - 1)
                    list.index--;
                
                serializedObject.Update();
            };
        }
    }
}