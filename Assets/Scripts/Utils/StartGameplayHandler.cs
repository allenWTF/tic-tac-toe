using Gameplay;
using JetBrains.Annotations;
using TMPro;
using UI.ModalWindow;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Handler of transitioning to core gameplay
    /// </summary>
    public class StartGameplayHandler : MonoBehaviour
    {
        [SerializeField] private Rules rules;
        [SerializeField] private SceneLoader sceneLoader;

        /// <summary>
        /// Activate modal window before going to core gameplay. This window includes input field to get some rules from user's input.
        /// </summary>
        [UsedImplicitly]
        public void GoToGameplay()
        {
            var actions = new ModalWindowAction[1];
            actions[0] = new ModalWindowAction("Играть", () =>
            {
                var input = ModalWindow.instance.GetInput();
                rules.FieldDimensions = int.Parse(input);
                sceneLoader.NexScene();
            });

            ModalWindow.instance.Show("Запуск игры", "Задайте размер игрового поля", actions, withInput: true,
                inputText: rules.FieldDimensions.ToString(), contentType: TMP_InputField.ContentType.IntegerNumber);
        }
    }
}