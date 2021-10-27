using JetBrains.Annotations;
using UI.ModalWindow;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Handler of fully exiting the game
    /// </summary>
    public class ExitHandler : MonoBehaviour
    {
        [SerializeField] private bool useEscapeKeyCode;
        
        private bool isExitRequested;
        
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape) || !useEscapeKeyCode) return;
            if (!isExitRequested)
            {
                Exit();
                return;
            }
                
            ModalWindow.instance.Close();
            isExitRequested = false;
        }

        /// <summary>
        /// Activate modal window with quitting the game action
        /// </summary>
        [UsedImplicitly]
        public void Exit()
        {
            var actions = new ModalWindowAction[1];
            actions[0] = new ModalWindowAction("Да", Application.Quit);
            ModalWindow.instance.Show("Выход из игры", "Вы действительно хотите выйти из игры?", actions, true);
            isExitRequested = true;
        }
    }
}
