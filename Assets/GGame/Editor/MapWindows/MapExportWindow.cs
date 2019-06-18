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
            string path = Path.Combine("Assets/GGame/Res/MapConfig", $"map_config_{strId}.xml");

            foreach (var go in gos)
            {
                ExportGameObject(go, doc, rootNode);
            }
            doc.Save(path);
            
            AssetDatabase.Refresh();
        };
        

    }

    void ExportGameObject(GameObject go, XmlDocument doc, XmlNode rootNode)
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
                    var entityNode = doc.CreateElement("Entity");
                    rootNode.AppendChild(entityNode);
                    var bocC = doc.CreateElement("BoxShaperComponent");
                    entityNode.AppendChild(bocC);
                    var sizeX = doc.CreateAttribute("sizeX");
                    var sizeY = doc.CreateAttribute("sizeY");
                    var sizeZ = doc.CreateAttribute("sizeZ");

                    sizeX.Value = box.bounds.size.x.ToString();
                    sizeY.Value = box.bounds.size.y.ToString();
                    sizeZ.Value = box.bounds.size.z.ToString();

                    bocC.Attributes.Append(sizeX);
                    bocC.Attributes.Append(sizeY);
                    bocC.Attributes.Append(sizeZ);
                    
                    var centerX = doc.CreateAttribute("centerX");
                    var centerY = doc.CreateAttribute("centerY");
                    var centerZ = doc.CreateAttribute("centerZ");

                    centerX.Value = box.bounds.center.x.ToString();
                    centerY.Value = box.bounds.center.y.ToString();
                    centerZ.Value = box.bounds.center.z.ToString();

                    bocC.Attributes.Append(centerX);
                    bocC.Attributes.Append(centerY);
                    bocC.Attributes.Append(centerZ);
                    
                    var positionX = doc.CreateAttribute("positionX");
                    var positionY = doc.CreateAttribute("positionY");
                    var positionZ = doc.CreateAttribute("positionY");

                    positionX.Value = go.transform.position.x.ToString();
                    positionY.Value = go.transform.position.y.ToString();
                    positionZ.Value = go.transform.position.z.ToString();

                    entityNode.Attributes.Append(positionX);
                    entityNode.Attributes.Append(positionY);
                    entityNode.Attributes.Append(positionZ);
                    
                    var eulerX = doc.CreateAttribute("eulerX");
                    var eulerY = doc.CreateAttribute("eulerY");
                    var eulerZ = doc.CreateAttribute("eulerZ");
                    
                    eulerX.Value = (go.transform.rotation.eulerAngles.x / 180 * Mathf.PI).ToString();
                    eulerY.Value = (go.transform.rotation.eulerAngles.y / 180 * Mathf.PI).ToString();
                    eulerZ.Value = (go.transform.rotation.eulerAngles.z/180 * Mathf.PI).ToString();

                    entityNode.Attributes.Append(eulerX);
                    entityNode.Attributes.Append(eulerY);
                    entityNode.Attributes.Append(eulerZ);

                   
                    
                    
                    

                }
                    break;
            }
        }

        for (int i = 0; i < go.transform.childCount; i++)
        {
            var child = go.transform.GetChild(i).gameObject;
            
            ExportGameObject(child, doc, rootNode);
        }
    }
}