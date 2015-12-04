using UnityEngine;
using UnityEditor;
using System;

public class EditorPopups : EditorWindow
{
    public event System.Action<bool> OnChosen;
    string popText = "";
    string trueText = "Yes";
    string falseText = "No";

    public void SetValue(string text, string accept, string no)
    {
        this.popText = text;
        this.trueText = accept;
        this.falseText = no;
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label(popText);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(trueText))
        {
            if (OnChosen != null)
                OnChosen(true);
            this.Close();
        }
        if (GUILayout.Button(falseText))
        {
            if (OnChosen != null)
                OnChosen(false);
            this.Close();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}


public class EditorGUIExtension
{
    public static void OpenPopup(string text, string trueText, string falseText, Action<bool> onChosen)
    {
        EditorPopups popwindow = (EditorPopups)EditorWindow.GetWindow(typeof(EditorPopups));
        popwindow.titleContent = new GUIContent("Modal");
        popwindow.SetValue(text, trueText, falseText);
        popwindow.OnChosen += onChosen;
    }

    public static Enum EnumToolbar(Enum selected)
    {
        string[] toolbar = System.Enum.GetNames(selected.GetType());
        Array values = System.Enum.GetValues(selected.GetType());

        for (int i = 0; i < toolbar.Length; i++)
        {
            string toolname = toolbar[i];
            toolname = toolname.Replace("_", " ");
            toolbar[i] = toolname;
        }

        int selected_index = 0;
        while (selected_index < values.Length)
        {
            if (selected.ToString() == values.GetValue(selected_index).ToString())
            {
                break;
            }
            selected_index++;
        }
        selected_index = GUILayout.Toolbar(selected_index, toolbar);
        return (Enum)values.GetValue(selected_index);
    }

    public static Enum EnumComboBox(string label, Enum selected)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(label);
        var result = EnumComboBox(selected);
        EditorGUILayout.EndHorizontal();
        return result;
    }

    public static Enum EnumComboBox(Enum selected)
    {
        string[] toolbar = System.Enum.GetNames(selected.GetType());
        Array values = System.Enum.GetValues(selected.GetType());


        int selected_index = 0;
        while (selected_index < values.Length)
        {
            if (selected.ToString() == values.GetValue(selected_index).ToString())
            {
                break;
            }
            selected_index++;
        }
        selected_index = EditorGUILayout.Popup(selected_index, toolbar);
        return (Enum)values.GetValue(selected_index);
    }

    public static string FileSelector(string name, string path, string extension)
    {
        EditorGUILayout.BeginHorizontal();
        string filepath = EditorGUILayout.TextField(name, path);
        if (GUILayout.Button("Browse", GUILayout.MaxWidth(60)))
        {
            filepath = EditorUtility.OpenFilePanel(name, path, extension);
        }
        EditorGUILayout.EndHorizontal();
        return filepath;
    }

    public static string FolderSelector(string name, string path)
    {
        EditorGUILayout.BeginHorizontal();
        string filepath = EditorGUILayout.TextField(name, path);
        if (GUILayout.Button("Browse", GUILayout.MaxWidth(60)))
        {
            filepath = EditorUtility.SaveFolderPanel(name, path, "Folder");
        }
        EditorGUILayout.EndHorizontal();
        return filepath;
    }

    public static void ArrayField(SerializedProperty property)
    {
        //EditorGUIUtility.LookLikeInspector();
        bool wasEnabled = GUI.enabled;
        int prevIdentLevel = EditorGUI.indentLevel;

        bool childrenAreExpanded = true;
        int propertyStartingDepth = property.depth;
        while (property.NextVisible(childrenAreExpanded) && propertyStartingDepth < property.depth)
        {
            childrenAreExpanded = EditorGUILayout.PropertyField(property);
        }

        EditorGUI.indentLevel = prevIdentLevel;
        GUI.enabled = wasEnabled;
    }

    public static string[] ArrayFoldout(string label, string[] array, ref bool foldout)
    {
        EditorGUILayout.BeginVertical();
        //EditorGUIUtility.LookLikeInspector();
        foldout = EditorGUILayout.Foldout(foldout, label);
        string[] newArray = array;
        if (foldout)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();
            int arraySize = EditorGUILayout.IntField("Size", array.Length);
            if (arraySize != array.Length)
                newArray = new string[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                string entry = "";
                if (i < array.Length)
                    entry = array[i];
                newArray[i] = EditorGUILayout.TextField("Element " + i, entry);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        return newArray;
    }

    public static bool ToggleButton(bool state, string label)
    {
        BuildStyle();

        bool out_bool = false;

        if (state)
            out_bool = GUILayout.Button(label, toggled_style);
        else
            out_bool = GUILayout.Button(label);

        if (out_bool)
            return !state;
        else
            return state;
    }


    static GUIStyle toggled_style;
    public GUIStyle StyleButtonToggled
    {
        get
        {
            BuildStyle();
            return toggled_style;
        }
    }

    static GUIStyle labelText_style;
    public static GUIStyle StyleLabelText
    {
        get
        {
            BuildStyle();
            return labelText_style;
        }
    }

    private static void BuildStyle()
    {
        if (toggled_style == null)
        {
            toggled_style = new GUIStyle(GUI.skin.button);
            toggled_style.normal.background = toggled_style.onActive.background;
            toggled_style.normal.textColor = toggled_style.onActive.textColor;
        }
        if (labelText_style == null)
        {
            labelText_style = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).textField);
            labelText_style.normal = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).button.onNormal;
        }
    }
}