using System;
using UnityEngine;

namespace Api
{
    [Serializable]
    public class RecordResultDto
    {
        [SerializeField]
        private string playerName;
        
        [SerializeField]
        private bool animalKill;

        [SerializeField]
        private bool pedestrianKill;

        [SerializeField]
        private bool carAccident;

        [SerializeField]
        private bool illegalLaneChange;

        [SerializeField]
        private bool signalViolation;

        [SerializeField]
        private bool centerLineViolation;

        [SerializeField]
        private bool offPath;

        [SerializeField]
        private bool maxSpeed;

        [SerializeField]
        private bool wiperCount;

        [SerializeField]
        private bool klaxonCount;

        [SerializeField]
        private bool clearCount;

        [SerializeField]
        private bool minClearTime;

        [SerializeField]
        private bool achievementCount;

        public string PlayerName => playerName;

        public bool AnimalKill => animalKill;

        public bool PedestrianKill => pedestrianKill;

        public bool CarAccident => carAccident;

        public bool IllegalLaneChange => illegalLaneChange;

        public bool SignalViolation => signalViolation;

        public bool CenterLineViolation => centerLineViolation;

        public bool OffPath => offPath;

        public bool MaxSpeed => maxSpeed;

        public bool WiperCount => wiperCount;

        public bool KlaxonCount => klaxonCount;

        public bool ClearCount => clearCount;

        public bool MinClearTime => minClearTime;

        public bool AchievementCount => achievementCount;

        public RecordResultDto(string playerName, bool animalKill, bool pedestrianKill, bool carAccident, bool illegalLaneChange, bool signalViolation, bool centerLineViolation, bool offPath, bool maxSpeed, bool wiperCount, bool klaxonCount, bool clearCount, bool minClearTime, bool achievementCount)
        {
            this.playerName = playerName;
            this.animalKill = animalKill;
            this.pedestrianKill = pedestrianKill;
            this.carAccident = carAccident;
            this.illegalLaneChange = illegalLaneChange;
            this.signalViolation = signalViolation;
            this.centerLineViolation = centerLineViolation;
            this.offPath = offPath;
            this.maxSpeed = maxSpeed;
            this.wiperCount = wiperCount;
            this.klaxonCount = klaxonCount;
            this.clearCount = clearCount;
            this.minClearTime = minClearTime;
            this.achievementCount = achievementCount;
        }

        public override string ToString()
        {
            return playerName + ": " + animalKill + " " + pedestrianKill + " " + carAccident +
                   " " + illegalLaneChange + " " + signalViolation + " " + centerLineViolation +
                   " " + offPath + " " + maxSpeed + " " + wiperCount + " " + klaxonCount +
                   " " + clearCount + " " + minClearTime + " " + achievementCount;
        }

        public int GetCount()
        {
            int sum = 0;

            if (animalKill) sum++;
            if (pedestrianKill) sum++;
            if (carAccident) sum++;
            if (illegalLaneChange) sum++;
            if (signalViolation) sum++;
            if (centerLineViolation) sum++;
            if (offPath) sum++;
            if (maxSpeed) sum++;
            if (wiperCount) sum++;
            if (klaxonCount) sum++;
            if (clearCount) sum++;
            if (minClearTime) sum++;
            if (achievementCount) sum++;

            return sum;
        }
    }
}