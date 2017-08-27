using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {

    public partial class CurrentScoreView : RavenhillGameBehaviour {

#pragma warning disable 0649
        [SerializeField]
        private Text m_RankText;

        [SerializeField]
        private Text m_ScoreText;
#pragma warning restore 0649

        private Text rankText => m_RankText;
        private Text scoreText => m_ScoreText;
    }
}
