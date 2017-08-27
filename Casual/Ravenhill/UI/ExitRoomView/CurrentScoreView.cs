namespace Casual.Ravenhill.UI {

    public partial class CurrentScoreView : RavenhillGameBehaviour {

        public void Setup(SearchSession session ) {
            rankText.text = netService.GetRank(session).ToString();
            scoreText.text = session.currentScore.ToString();
        }


    }
}
