using System;

namespace Api
{
    [Serializable]
    public class RecordResultDto
    {
        private string playerName;
        
        private bool animalKill;

        private bool pedestrianKill;

        private bool carAccident;

        private bool illegalLaneChange;

        private bool signalViolatio;

        private bool centerLineViolation;

        private bool offPath;

        private bool maxSpeed;

        private bool wiperCount;

        private bool klaxonCount;

        private bool clearCount;

        private bool minClearTime;

        private bool achievementCount;

        public string PlayerName => playerName;

        public bool AnimalKill => animalKill;

        public bool PedestrianKill => pedestrianKill;

        public bool CarAccident => carAccident;

        public bool IllegalLaneChange => illegalLaneChange;

        public bool SignalViolatio => signalViolatio;

        public bool CenterLineViolation => centerLineViolation;

        public bool OffPath => offPath;

        public bool MaxSpeed => maxSpeed;

        public bool WiperCount => wiperCount;

        public bool KlaxonCount => klaxonCount;

        public bool ClearCount => clearCount;

        public bool MinClearTime => minClearTime;

        public bool AchievementCount => achievementCount;

        public RecordResultDto(string playerName, bool animalKill, bool pedestrianKill, bool carAccident, bool illegalLaneChange, bool signalViolatio, bool centerLineViolation, bool offPath, bool maxSpeed, bool wiperCount, bool klaxonCount, bool clearCount, bool minClearTime, bool achievementCount)
        {
            this.playerName = playerName;
            this.animalKill = animalKill;
            this.pedestrianKill = pedestrianKill;
            this.carAccident = carAccident;
            this.illegalLaneChange = illegalLaneChange;
            this.signalViolatio = signalViolatio;
            this.centerLineViolation = centerLineViolation;
            this.offPath = offPath;
            this.maxSpeed = maxSpeed;
            this.wiperCount = wiperCount;
            this.klaxonCount = klaxonCount;
            this.clearCount = clearCount;
            this.minClearTime = minClearTime;
            this.achievementCount = achievementCount;
        }
    }
}