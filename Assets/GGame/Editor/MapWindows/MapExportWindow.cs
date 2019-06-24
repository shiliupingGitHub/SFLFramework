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
        Collider2D collider = go.GetComponent<Collider2D>();

        if (null != collider)
        {
            switch (collider)
            {
              
                    
                case BoxCollider2D box:
                {
                    
                    var block = doc.CreateElement("Box");
                    blocksNode.AppendChild(block);

                    var postionAttr = doc.CreateAttribute("postion");
                    postionAttr.Value = $"{box.bounds.center.x},{box.bounds.center.y}";

                    block.Attributes.Append(postionAttr);
                    
                    var widthAttr = doc.CreateAttribute("width");
                    widthAttr.Value =$"{box.bounds.size.x}";

                    block.Attributes.Append(widthAttr);
                    
                    var heightAttr = doc.CreateAttribute("height");
                    heightAttr.Value =$"{box.bounds.size.y}";

                    block.Attributes.Append(heightAttr);

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