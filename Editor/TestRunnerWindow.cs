using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PowerTest
{
    public class TestRunnerWindow : EditorWindow
    {
        private Vector2 scrollPosition;
        private List<string> testResults = new List<string>();
        private bool isGameRunning = false;
        private Texture2D passIcon; // Иконка для успешного теста
        private Texture2D failIcon; // Иконка для неудачного теста

        private Dictionary<string, bool> showDetails = new Dictionary<string, bool>();

        [MenuItem("Power Test/Run Tests")]
        public static void ShowWindow()
        {
            GetWindow<TestRunnerWindow>("Power Test");
        }

        void OnEnable()
        {
            passIcon = Resources.Load<Texture2D>("passIcon");
            failIcon = Resources.Load<Texture2D>("failIcon");

            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
        void OnDisable()
        {
            // Отписываемся от события при выключении окна
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        // Обработчик изменения состояния Play Mode
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                isGameRunning = false;
                Repaint();
            }
        }
        private void ClearTestResults()
        {
            testResults.Clear();
            TestRunner.ClearTestResults(); // Убедитесь, что этот метод существует и корректно очищает результаты тестов в TestRunner
            showDetails.Clear();
            Repaint();
        }

        void OnGUI()
        {
            if (GUILayout.Button("Start Game"))
            {
                StartGame();
            }

            if (isGameRunning && GUILayout.Button("Run Tests"))
            {
                RunTests();
            }

            if (GUILayout.Button("Clear Log"))
            {
                ClearTestResults();
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (var result in testResults)
            {
                // Если результат содержит разделитель, отобразить его как заголовок
                if (result.StartsWith("-----"))
                {
                    EditorGUILayout.LabelField(result, EditorStyles.boldLabel);
                    continue;
                }
                string testName = result.Split(':')[0]; // Объявляем здесь, используем ниже
                bool isFailedTest = result.Contains("Failed");

                // Добавляем в словарь, если это неудачный тест и его еще нет в словаре
                if (isFailedTest && !showDetails.ContainsKey(testName))
                {
                    showDetails.Add(testName, false);
                }

                EditorGUILayout.BeginHorizontal();
                if (result.Contains("Passed"))
                {
                    if (passIcon != null)
                        GUILayout.Label(passIcon, GUILayout.Width(20), GUILayout.Height(20));
                    EditorGUILayout.LabelField(result);
                }
                else if (isFailedTest)
                {
                    if (failIcon != null)
                        GUILayout.Label(failIcon, GUILayout.Width(20), GUILayout.Height(20));

                    // Переключатель для показа/скрытия деталей ошибки
                    showDetails[testName] = EditorGUILayout.Foldout(showDetails[testName], testName);
                }
                EditorGUILayout.EndHorizontal();

                // Показываем детали ошибки только если переключатель активирован
                if (isFailedTest && showDetails[testName])
                {
                    // Используйте GUIStyle для word-wrapped textArea
                    GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea) { wordWrap = true };
                    EditorGUILayout.TextArea(result, textAreaStyle, GUILayout.ExpandHeight(true));
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void StartGame()
        {
            EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Steampunk_Computer_Collection/Scenes/MechaLearnVR.unity");
            EditorApplication.EnterPlaymode();
            isGameRunning = true;
        }

        private async void RunTests()
        {
            if (!EditorApplication.isPlaying)
            {
                Debug.LogError("The game is not running. Please start the game first.");
                return;
            }

            ClearTestResults(); // Замените testResults.Clear() на метод ClearTestResults
            await TestRunner.RegisterAndRunTests();

            // Убедитесь, что TestRunner.TestResults уже содержит обновленные результаты
            testResults.AddRange(TestRunner.TestResults);

            Repaint();
        }
    }
}
