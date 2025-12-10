using UnityEditor;
using UnityEngine;
using System.Reflection;

[CustomEditor(typeof(MonoBehaviour), true)]
public class ProButtonDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MonoBehaviour mono = (MonoBehaviour)target;

        // Lấy toàn bộ method của script
        MethodInfo[] methods = mono.GetType().GetMethods(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (MethodInfo method in methods)
        {
            var buttonAttr = method.GetCustomAttribute<ProButtonAttribute>();

            if (buttonAttr != null)
            {
                string btnName = string.IsNullOrEmpty(buttonAttr.ButtonName)
                    ? method.Name
                    : buttonAttr.ButtonName;

                if (GUILayout.Button(btnName))
                {
                    method.Invoke(mono, null);
                }
            }
        }
    }
}
