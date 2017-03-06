using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KinematicBlock))]
[CanEditMultipleObjects]

public class KinematicBlockEditor : Editor
{
    KinematicBlock subject;
    SerializedProperty typeOfAngle;
    SerializedProperty angle;

    SerializedProperty delay;
    SerializedProperty expansion;
    SerializedProperty expansionTime;
    SerializedProperty startChain;
    SerializedProperty endChain;
    SerializedProperty rotation;


    //Передаём этому скрипту компонент и необходимые в редакторе поля
    void OnEnable()
    {
        subject = target as KinematicBlock;

        typeOfAngle = serializedObject.FindProperty("typeOfAngle");
        angle = serializedObject.FindProperty("angle");
        rotation = serializedObject.FindProperty("rotation");
        delay = serializedObject.FindProperty("delay");
        expansion = serializedObject.FindProperty("expansion");
        expansionTime = serializedObject.FindProperty("expansionTime");
        startChain = serializedObject.FindProperty("startChain");
        endChain = serializedObject.FindProperty("endChain");
    }

    //Переопределяем событие отрисовки компонента
    public override void OnInspectorGUI()
    {
        //Метод обязателен в начале. После него редактор компонента станет пустым и
        //далее мы с нуля отрисовываем его интерфейс.
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        //Вывод в редактор выпадающего меню
        EditorGUILayout.PropertyField(typeOfAngle);
        //Проверка выбранного пункта в выпадающем меню, 
        if (subject.typeOfAngle == KinematicBlock.AngleType.Custom)
        {
            //Вывод в редактор слайдера
            EditorGUILayout.Slider(angle, 0, 359, new GUIContent("Angle"));
            //compName.stringValue = "First";

        }
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(delay);
        EditorGUILayout.PropertyField(expansion);
        EditorGUILayout.PropertyField(expansionTime);

        EditorGUILayout.Space();
        EditorGUILayout.Slider(rotation, -100, 100, new GUIContent("Rotations"));

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(startChain, true);
        EditorGUILayout.PropertyField(endChain, true);

        //DrawDefaultInspector();



        //Метод обязателен в конце
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}
