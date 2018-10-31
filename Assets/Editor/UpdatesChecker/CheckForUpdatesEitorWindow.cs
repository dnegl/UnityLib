using UnityEngine;
using UnityEditor;
using System;

public class CheckForUpdatesEitorWindow : EditorWindow
{
    private UpdateState _state = UpdateState.Idle;
    private FrameworkUpdatesChecker _frameworkUpdatesChecker = new FrameworkUpdatesChecker();

    [MenuItem("Frame8Tools/Check for Updates")]
    static void Init()
    {
        CheckForUpdatesEitorWindow window = (CheckForUpdatesEitorWindow)GetWindow(typeof(CheckForUpdatesEitorWindow));
        window.Show();
    }

    void OnGUI()
    {
        DrawButtons();
    }

    private void DrawButtons()
    {
        if ((_state & UpdateState.Idle) == UpdateState.Idle)
        {
            if (GUILayout.Button("Check for updates"))
                OnCheckPressed();
        }
        if ((_state & UpdateState.Updated) == UpdateState.Updated)
        {
            EditorGUILayout.LabelField("you have actual version of framework installed!");
        }
        if (_state == UpdateState.NeedToUpdate)
        {
            EditorGUILayout.LabelField("There is a new version of framework available!");
            if (GUILayout.Button("Update now"))
            {
                OnUpdatePressed();
                _state = UpdateState.Updated | UpdateState.Idle;
            }
        }
    }

    private void OnCheckPressed()
    {
        _state = _frameworkUpdatesChecker.Ckeck() ? UpdateState.NeedToUpdate : UpdateState.Idle | UpdateState.Updated;
    }

    private void OnUpdatePressed()
    {
        _frameworkUpdatesChecker.Update();
    }
}

[Flags]
public enum UpdateState
{
    Idle = 1,
    Updated = 2,
    NeedToUpdate = 4
}

