using System;
using UnityEngine;

namespace Api
{
    [Serializable]
    public class RecordDto
    {
        [SerializeField]
        private string playerName;
        
        /**
         * 게임이 종료된 원인
         * 0 - 클리어
         * 1 - 동물 킬
         * 2 - 보행자 킬
         * 3 - 차 사고
         * 4 - 깜빡이 없이 차선 변경
         * 5 - 신호 위반(신호등)
         * 6 - 중앙선 침범
         * 7 - 길 벗어남
         */
        [SerializeField]
        private int type;
        [SerializeField]
        private int maxSpeed;
        [SerializeField]
        private int wiperCount;
        [SerializeField]
        private int klaxonCount;
        
        [SerializeField]
        private float clearTime;

        public RecordDto(string playerName, int type, int maxSpeed, int wiperCount, int klaxonCount, float clearTime)
        {
            this.playerName = playerName;
            this.type = type;
            this.maxSpeed = maxSpeed;
            this.wiperCount = wiperCount;
            this.klaxonCount = klaxonCount;
            this.clearTime = clearTime;
        }
    }
}