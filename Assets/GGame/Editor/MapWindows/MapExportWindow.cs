using System;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class MapExportWindow : EditorWindow
{
    [MenuItem("Window/UIElements/MapExportWindow")]
    public static void ShowExample()
    {
        MapExportWindow wnd = GetWindow<MapExportWindow>();
        wnd.titleContent = new GUIContent("MapExportWindow");
    }

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/GGame/Editor/MapWindows/MapExportWindow.uxml");
        VisualElement labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);

        var ti = labelFromUXML.Query<TextField>("id").First();
        var tsizeX = labelFromUXML.Query<TextField>("x").First();
        var tsizeY = labelFromUXML.Query<TextField>("y").First();
        
        labelFromUXML.Query<Button>("btn_export").First().clickable.clicked += delegate
        {
            var strId = ti.text;
            var id = Convert.ToInt32(strId);

            var gos = EditorSceneManager.GetActiveScene().GetRootGameObjects();
            
            XmlDocument doc = new XmlDocument();

            var rootNode = doc.CreateElement("Map");
            doc.AppendChild(rootNode);
            
            //blockInfo

            var blocksNode = doc.CreateElement("Blocks");
            rootNode.AppendChild(blocksNode);

            var sizeXAttr =doc.CreateAttribute("x");
            var sizeYAttr = doc.CreateAttribute("y");

            sizeXAttr.Value = tsizeX.text;
            sizeYAttr.Value = tsizeY.text;

            blocksNode.Attributes.Append(sizeXAttr);
            blocksNode.Attributes.Append(sizeYAttr);
            string path = Path.Combine("Assets/GGame/Res/MapConfig", $"map_config_{strId}.xml");

            foreach (var go in gos)
            {
                ExportGameObject(go, doc, blocksNode);
            }
            doc.Save(path);
            
            AssetDatabase.Refresh();
        };
        

    }

    void ExportGameObject(GameObject go, XmlDocument doc, XmlNode blocksNode)
    {
        if(!go.activeSelf)
            return;
        Collider collider = go.GetComponent<Collider>();

        if (null != collider)
        {
            switch (collider)
            {
                case BoxCollider box:
                {
                    var block = doc.CreateElement("Block");
                    blocksNode.AppendChild(block);

                    var MinAttr = doc.CreateAttribute("min");
                    MinAttr.Value = $"{box.bounds.min.x},{box.bounds.min.y}";

                    block.Attributes.Append(MinAttr);
                    
                    var MaxAttr = doc.CreateAttribute("max");
                    MaxAttr.Value =$"{box.bounds.max.x},{box.bounds.max.y}";

                    block.Attributes.Append(MaxAttr);

                }
                    break;
            }
        }

        for (int i = 0; i < go.transform.childCount; i++)
        {
            var child = go.transform.GetChild(i).gameObject;
            
            ExportGameObject(child, doc, blocksNode);
        }
    }
}