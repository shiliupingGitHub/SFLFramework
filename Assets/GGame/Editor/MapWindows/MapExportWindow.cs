using System;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.AI;


public class MapExportWindow : EditorWindow
{
    [MenuItem("Tools/MapExportWindow")]
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

            var NavMeshNode = doc.CreateElement("NavMesh");
            rootNode.AppendChild(NavMeshNode);
            
            string path = Path.Combine("Assets/GGame/Res/MapConfig", $"map_config_{strId}.xml");
            
            doc.Save(path);
            
            AssetDatabase.Refresh();
        };
        

    }

   
}