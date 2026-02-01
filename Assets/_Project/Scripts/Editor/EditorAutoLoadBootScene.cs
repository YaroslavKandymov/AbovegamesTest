using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Project.Editor
{
    public class EditorAutoLoadBootScene
    {
        private const string BootScenePath = "Assets/_Project/Scenes/Bootstrap.unity";

        private static string prevScenePath;

        [InitializeOnLoadMethod]
        private static void InitializeEditorMode()
        {
            EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;
        }

        private static void EditorApplication_PlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                var activeScene = SceneManager.GetActiveScene();
                EditorPrefs.SetString("PREV_PATH", activeScene.path);
                EditorPrefs.SetInt("NEXT_SCENE", activeScene.buildIndex);

                EditorSceneManager.SaveOpenScenes();
                EditorSceneManager.OpenScene(BootScenePath, OpenSceneMode.Single);
            }
            else if (state == PlayModeStateChange.EnteredEditMode)
            {
                EditorSceneManager.OpenScene(EditorPrefs.GetString("PREV_PATH"), OpenSceneMode.Single);
            }
        }
    }
}