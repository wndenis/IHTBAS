using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KinematicBlock))]
[CanEditMultipleObjects]

public class KinematicBlockEditor : Editor
{
    //KinematicBlock subject;
    SerializedProperty angleType;

    SerializedProperty delay;
    SerializedProperty expansion;
    SerializedProperty expansionTime;
    SerializedProperty startChain;
    SerializedProperty endChain;
    SerializedProperty reverseAtEnd;
    SerializedProperty reverseCollider;
    SerializedProperty reverseType;
    SerializedProperty reverseTriggerCondition;
    SerializedProperty cycle;
    SerializedProperty delayAtEnd;
    SerializedProperty mayKill;
    SerializedProperty killZoneTrfm;
    SerializedProperty killDelay;


    //Передаём этому скрипту компонент и необходимые в редакторе поля
    void OnEnable()
    {
        //subject = target as KinematicBlock;

        angleType = serializedObject.FindProperty("angleType");
        delay = serializedObject.FindProperty("delay");
        expansion = serializedObject.FindProperty("expansion");
        expansionTime = serializedObject.FindProperty("expansionTime");
        startChain = serializedObject.FindProperty("startChain");
        endChain = serializedObject.FindProperty("endChain");
        reverseAtEnd = serializedObject.FindProperty("reverseAtEnd");
        reverseCollider = serializedObject.FindProperty("reverseCollider");
        reverseTriggerCondition = serializedObject.FindProperty("reverseTriggerCondition");
        reverseType = serializedObject.FindProperty("reverseType");
        cycle = serializedObject.FindProperty("cycle");
        delayAtEnd = serializedObject.FindProperty("delayAtEnd");
        mayKill = serializedObject.FindProperty("mayKill");
        killZoneTrfm = serializedObject.FindProperty("killZoneTrfm");
        killDelay = serializedObject.FindProperty("killDelay");
    }

    //Переопределяем событие отрисовки компонента
    public override void OnInspectorGUI()
    {
        //Метод обязателен в начале. После него редактор компонента станет пустым и
        //далее мы с нуля отрисовываем его интерфейс.
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(angleType);
        /*
        //Проверка выбранного пункта в выпадающем меню, 
        if (subject.typeOfAngle == KinematicBlock.AngleType.Custom)
        {
            //Вывод в редактор слайдера
            EditorGUILayout.Slider(angle, 0, 359, new GUIContent("Angle"));
            //compName.stringValue = "First";
        }*/
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(delay);
        EditorGUILayout.PropertyField(expansion);
        EditorGUILayout.PropertyField(expansionTime);

        EditorGUILayout.Space();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(reverseAtEnd);
        if (reverseAtEnd.boolValue)
        {
            EditorGUILayout.PropertyField(reverseType);
            if(reverseType.enumValueIndex == 0)
            {
                EditorGUILayout.PropertyField(delayAtEnd);
            }
            else
            {
                EditorGUILayout.PropertyField(reverseTriggerCondition);
                EditorGUILayout.PropertyField(reverseCollider);
            }
            EditorGUILayout.PropertyField(cycle);
        }
        //if (!cycle.boolValue)
        //{
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(startChain, true);
            EditorGUILayout.PropertyField(endChain, true);
        //}

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(mayKill);
        if (mayKill.boolValue)
        {
            EditorGUILayout.PropertyField(killDelay);
            EditorGUILayout.PropertyField(killZoneTrfm);
        }


        //DrawDefaultInspector();



        //Метод обязателен в конце
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}
