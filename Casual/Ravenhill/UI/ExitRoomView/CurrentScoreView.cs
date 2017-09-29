namespace Casual.Ravenhill.UI {

    public partial class CurrentScoreView : RavenhillGameBehaviour {

        public void Setup(SearchSession session ) {
            rankText.text = netService.GetLocalPlayerRank(session).ToString();
            scoreText.text = session.currentScore.ToString();
        }


    }
}
