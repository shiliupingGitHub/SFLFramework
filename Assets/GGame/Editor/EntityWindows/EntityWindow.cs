using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class EntityWindow : EditorWindow
{
    [MenuItem("Window/UIElements/EntityWindow")]
    public static void ShowExample()
    {
        EntityWindow wnd = GetWindow<EntityWindow>();
        wnd.titleContent = new GUIContent("EntityWindow");
    }

    private VisualElement _entity_list;
    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        
        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/GGame/Editor/EntityWindows/EntityWindow.uxml");
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/GGame/Editor/EntityWindows/EntityWindow.uss");
        VisualElement _entity_list = visualTree.CloneTree();
        _entity_list.styleSheets.Add(styleSheet);
        root.Add(_entity_list);
        
       
    }
}