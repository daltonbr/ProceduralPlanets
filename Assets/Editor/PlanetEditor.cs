using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    private Planet _planet;
    private Editor _colorEditor;
    private Editor _shapeEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                _planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            _planet.GeneratePlanet();
        }

        DrawSettingsEditor(_planet.shapeSettings, _planet.OnShapeSettingsUpdate, ref _planet.shapeSettingsFoldout, ref _shapeEditor);
        DrawSettingsEditor(_planet.colorSettings, _planet.OnColorSettingsUpdate, ref _planet.colorSettingsFoldout, ref _colorEditor);
    }

    private void OnEnable()
    {
        _planet = (Planet) target;
    }

    private void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings == null) return;

        foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            if (!foldout) return;
            CreateCachedEditor(settings, null, ref editor);
            editor.OnInspectorGUI();

            if (check.changed)
            {
                onSettingsUpdated?.Invoke();
            }
        }
    }
}
