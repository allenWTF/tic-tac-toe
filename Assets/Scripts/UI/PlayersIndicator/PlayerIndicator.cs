using TMPro;
using UnityEngine.UI;

namespace UI.PlayersIndicator
{
    public class PlayerIndicator
    {
        private TMP_Text nameLabel;
        private Image activeBorder;

        public PlayerIndicator(TMP_Text nameLabel, Image activeBorder)
        {
            this.nameLabel = nameLabel;
            this.activeBorder = activeBorder;
        }

        public void SetName(string newName)
        {
            nameLabel.text = newName;
        }

        public void ToggleActive(bool value)
        {
            activeBorder.enabled = value;
        }
    }
}