using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using PlayerData;
using UI.ModalWindow;
using UI.PlayersIndicator;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;

namespace Gameplay
{
    /// <summary>
    /// Core manager of all game-related processes
    /// </summary>
    [RequireComponent(typeof(PlayersIndicatorManager))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private FieldGenerator fieldGenerator;
        [SerializeField] private GameTimer gameTimer;
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] private Rules rules;
        [SerializeField] private GameObject menu;
        [SerializeField] private Sprite playAgainSprite;

        private const int SCORE_REWARD = 100;

        private Cell[,] cells;
        private CellState playerTeam;
        private CellState cpuTeam;
        private CellState currentTurn = CellState.Cross;
        private CellState winner = CellState.Empty;

        private Judge judge;
        private PlayerData.PlayerData playerData;
        private System.Random rand;
        private ExitHandler exitHandler;
        private PlayersIndicatorManager indicatorManager;

        private bool isPaused;

        private void Awake()
        {
            cells = fieldGenerator.GenerateField();
            judge = new Judge(cells);
            playerData = new PlayerData.PlayerData();
            rand = new System.Random();
            indicatorManager = GetComponent<PlayersIndicatorManager>();

            SetTeams();
        }

        private void OnEnable()
        {
            foreach (var cell in cells)
            {
                cell.onCellClick.AddListener(PlayerTurnHandler);
            }
        }

        private void OnDisable()
        {
            foreach (var cell in cells)
            {
                cell.onCellClick.RemoveListener(PlayerTurnHandler);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        /// <summary>
        /// Pause/unpause game
        /// </summary>
        [UsedImplicitly]
        public void TogglePause()
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        /// <summary>
        /// Activate modal window with return to main menu action
        /// </summary>
        [UsedImplicitly]
        public void ReturnToMainMenu()
        {
            var actions = new ModalWindowAction[1];
            actions[0] = new ModalWindowAction("Да", () => { sceneLoader.PrevScene(); });
            ModalWindow.instance.Show("Выход в главное меню", "Вы действительно хотите вернуться в главное меню?",
                actions, true);
        }

        /// <summary>
        /// Pause game
        /// </summary>
        private void Pause()
        {
            fieldGenerator.gameObject.SetActive(false);
            menu.gameObject.SetActive(true);
            gameTimer.Pause();
            isPaused = true;
        }

        /// <summary>
        /// Unpause game
        /// </summary>
        private void Resume()
        {
            fieldGenerator.gameObject.SetActive(true);
            menu.gameObject.SetActive(false);
            gameTimer.Play();
            isPaused = false;
        }

        /// <summary>
        /// Allocate teams for player and cpu and adjust UI accordingly
        /// </summary>
        private void SetTeams()
        {
            playerTeam = (CellState) rand.Next(1, 3);
            cpuTeam = (int) playerTeam / 2 == 0 ? CellState.Circle : CellState.Cross;

            indicatorManager.Init(playerTeam == CellState.Cross ? playerData.Name : "Компьютер",
                playerTeam == CellState.Circle ? playerData.Name : "Компьютер");

            if (currentTurn == cpuTeam)
            {
                StartCoroutine(CpuTurn());
            }
        }

        /// <summary>
        /// Handle player turn. Check if maybe game should be over already and if not pass the turn to cpu
        /// </summary>
        /// <param name="clickedCell">Cell on which player clicked</param>
        private void PlayerTurnHandler(Cell clickedCell)
        {
            if (currentTurn != playerTeam)
            {
                return;
            }

            clickedCell.SetState(playerTeam);
            if (CheckGameOver()) return;

            currentTurn = cpuTeam;
            indicatorManager.SetActiveIndicator(cpuTeam);
            StartCoroutine(CpuTurn());
        }

        /// <summary>
        /// Make cpu turn. Cpu turn is a random cell with state of Empty. Also check if maybe game should be over already and if not pass the turn to player
        /// </summary>
        /// <returns></returns>
        private IEnumerator CpuTurn()
        {
            yield return new WaitForSeconds((float) rand.NextDouble());

            var emptyCells = new List<Cell>();
            var fieldDimensions = rules.FieldDimensions;

            for (var i = 0; i < fieldDimensions; i++)
            {
                for (var j = 0; j < fieldDimensions; j++)
                {
                    if (cells[i, j].GetState() == CellState.Empty)
                    {
                        emptyCells.Add(cells[i, j]);
                    }
                }
            }

            var randomCellIndex = rand.Next(0, emptyCells.Count);
            emptyCells[randomCellIndex].SetState(cpuTeam);

            if (CheckGameOver()) yield return null;

            currentTurn = playerTeam;
            indicatorManager.SetActiveIndicator(playerTeam);
        }

        /// <summary>
        /// Check whether a game should be over or not
        /// </summary>
        /// <returns></returns>
        private bool CheckGameOver()
        {
            var (judgeResult, gameOverState) = judge.GetWinner();

            if (!gameOverState) return false;

            winner = judgeResult;
            End();
            return true;
        }

        #region Game over

        /// <summary>
        /// End the game
        /// </summary>
        private void End()
        {
            Debug.Log("Game over! Result: " + winner);
            gameTimer.Pause();
            GiveRewards();
            OpenGameOverWindow();
        }

        /// <summary>
        /// Give player rewards based on outcome of the game
        /// </summary>
        private void GiveRewards()
        {
            if (winner == CellState.Empty)
            {
                return;
            }

            if (winner == playerTeam)
            {
                playerData.Score += SCORE_REWARD;
                return;
            }

            playerData.Score -= SCORE_REWARD;
        }

        /// <summary>
        /// Save all ended match data
        /// </summary>
        private void SaveLastMatchData()
        {
            var lastMatch = new MatchData
            {
                description = ModalWindow.instance.GetInput(),
                duration = gameTimer.GetCurrentTime(),
                scorePrize = SCORE_REWARD
            };

            if (winner == CellState.Empty)
            {
                lastMatch.winner = "Ничья";
            }
            else
            {
                lastMatch.winner = winner == playerTeam ? playerData.Name : "Компьютер";
            }

            playerData.LastMatch = lastMatch;
        }

        /// <summary>
        /// Activate game over modal window
        /// </summary>
        private void OpenGameOverWindow()
        {
            var actions = new ModalWindowAction[2];
            actions[0] = new ModalWindowAction("Сыграть ещё", () =>
            {
                SaveLastMatchData();
                sceneLoader.ReloadScene();
            }, playAgainSprite);
            actions[1] = new ModalWindowAction("Выйти в меню", () =>
            {
                SaveLastMatchData();
                sceneLoader.PrevScene();
            });
            ModalWindow.instance.Show(GetGameOverHeader(), GetGameOverDescription(), actions, withCancel: false,
                withInput: true, inputText: "Неплохо сыграно!");
        }

        private string GetGameOverHeader()
        {
            if (winner == CellState.Empty)
            {
                return "Ничья!";
            }

            return winner == playerTeam ? $"Победил {playerData.Name}!" : "Победил компьютер!";
        }

        private string GetGameOverDescription()
        {
            var resultString = "Ваш рейтинг ";

            if (winner == CellState.Empty)
            {
                return resultString + "не изменился.\nОставьте отзыв о последнем матче: ";
            }

            resultString += winner == playerTeam ? "увеличился " : "уменьшился ";
            return resultString + "на " + SCORE_REWARD + " очков и теперь равен " + playerData.Score +
                   "\nОставьте отзыв о последнем матче:";
        }

        #endregion
    }
}