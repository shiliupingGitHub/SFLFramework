using System.IO;
using System.Xml;
using GGame.Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class MapWindow : EditorWindow
{
    [MenuItem("Window/UIElements/MapWindow")]
    public static void ShowExample()
    {
        MapWindow wnd = GetWindow<MapWindow>();
        wnd.titleContent = new GUIContent("MapWindow");
    }
    
    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;


        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/GGame/Editor/MapWindows/MapWindow.uxml");
        VisualElement labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);
        var tfId = labelFromUXML.Query<TextField>("tf_id");
        var btnExport = labelFromUXML.Query<Button>("btn_export");

        btnExport.First().clickable.clicked += () =>
        {
            var id = System.Convert.ToInt32(tfId.First().text);
            var path = $"Assets/GGame/Res/MapConfig/map_config_{id}.xml";
            var gos = EditorSceneManager.GetActiveScene().GetRootGameObjects();
            var document = new XmlDocument();
            var mapNode = document.CreateElement("Map");
            var mapNodeIDAttr =  document.CreateAttribute("ID");
            
            mapNodeIDAttr.Value = id.ToString();
            mapNode.Attributes.Append(mapNodeIDAttr);
            document.AppendChild(mapNode);

            foreach (var go in gos)
            {
                Search(go, document, mapNode);
            }
            
            document.Save(path);

        };

    }

    static void Search(GameObject go, XmlDocument doc, XmlNode mapNode)
    {
        if(!go.activeSelf)
            return;
        UnityEngine.Collider collider = go.GetComponent<UnityEngine.Collider>();

        if (null != collider)
        {
            Fix64 posX = (Fix64)go.transform.position.x;
            Fix64 posY = (Fix64)go.transform.position.y;
            Fix64 posZ = (Fix64)go.transform.position.z;

            Fix64 centerX = (Fix64) collider.bounds.center.x;
            Fix64 centerY = (Fix64) collider.bounds.center.y;
            Fix64 centerZ = (Fix64) collider.bounds.center.z;

            Fix64 sizeX = (Fix64) collider.bounds.size.x;
            Fix64 sizeY = (Fix64) collider.bounds.size.y;
            Fix64 sizeZ = (Fix64) collider.bounds.size.z;

            if (!collider.isTrigger)
            {
                var node = doc.CreateElement("Collider");

                var nameAttr = doc.CreateAttribute("name");

                nameAttr.Value = go.name;

                node.Attributes.Append(nameAttr);
                
                var posXAttr = doc.CreateAttribute("PosX");
                                             
                 posXAttr.Value = posX.ToString();
                 node.Attributes.Append(posXAttr);
                 
                 var posYAttr = doc.CreateAttribute("PosY");
                                             
                 posYAttr.Value = posY.ToString();
                 node.Attributes.Append(posYAttr);
                 
                 
                 var posZAttr = doc.CreateAttribute("PosZ");
                                             
                 posZAttr.Value = posZ.ToString();
                 node.Attributes.Append(posZAttr);
                 
                 
                 var CenterXAttr = doc.CreateAttribute("centerX");
                                             
                 CenterXAttr.Value = centerX.ToString();
                 node.Attributes.Append(CenterXAttr);
                 
                 
                 var CenterYAttr = doc.CreateAttribute("CenterY");
                                             
                 CenterYAttr.Value = centerY.ToString();
                 node.Attributes.Append(CenterYAttr);
                 
                 var CenterZAttr = doc.CreateAttribute("CenterZ");
                                             
                 CenterZAttr.Value = centerZ.ToString();
                 node.Attributes.Append(CenterZAttr);
                 
                                  
                 var sizeXAttr = doc.CreateAttribute("SizeX");
                                             
                 sizeXAttr.Value = sizeX.ToString();
                 node.Attributes.Append(sizeXAttr);
                 
                 var sizeYAttr = doc.CreateAttribute("SizeY");
                                             
                 sizeYAttr.Value = sizeY.ToString();
                 node.Attributes.Append(sizeYAttr);
                 
                 var sizeZAttr = doc.CreateAttribute("SizeZ");
                                             
                 sizeZAttr.Value = sizeZ.ToString();
                 node.Attributes.Append(sizeZAttr);

                 mapNode.AppendChild(node);
            }
        }

        for (int i = 0; i < go.transform.childCount; i++)
        {
            var child = go.transform.GetChild(i).gameObject;
            Search(child, doc, mapNode);
        }
        
        
    }
}