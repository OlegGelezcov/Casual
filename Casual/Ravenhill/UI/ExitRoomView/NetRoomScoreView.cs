using Casual.Ravenhill.Data;
using Casual.Ravenhill.Net;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public class NetRoomScoreView : RavenhillGameBehaviour {

#pragma warning disable 0649,0169
        [SerializeField]
        private GameObject m_ParentObject;

        [SerializeField]
        private GameObject m_MedalObject;

        [SerializeField]
        private Text m_RankText;

        [SerializeField]
        private Text m_ScoreText;

        [SerializeField]
        private Image m_PlayerIconImage;

        [SerializeField]
        private Text m_PlayerNameText;
#pragma warning restore 0649,0169

        private GameObject parentObject => m_ParentObject;
        private GameObject medalObject => m_MedalObject;
        private Text rankText => m_RankText;
        private Text scoreText => m_RankText;
        private Image playerIconImage => m_PlayerIconImage;
        private Text playerNameText => m_PlayerNameText;

        public void Setup(NetRoomScore roomScore) {
            if(roomScore.player.isValid) {
                parentObject.ActivateSelf();

                if(roomScore.rank == 1 ) {
                    medalObject.ActivateSelf();
                    rankText.text = string.Empty;
                } else {
                    medalObject.DeactivateSelf();
                    rankText.text = roomScore.rank.ToString();
                }

                scoreText.text = roomScore.score.ToString();

                AvatarData avatarData = resourceService.GetAvatarData(roomScore.player.avatarId);
                if(avatarData != null ) {
                    playerIconImage.overrideSprite = resourceService.GetSprite(avatarData);
                } else {
                    playerIconImage.overrideSprite = resourceService.transparent;
                }

                playerNameText.text = roomScore.player.name;

            } else {
                parentObject.DeactivateSelf();
            }
        }

    }
}
