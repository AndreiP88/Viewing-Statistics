namespace libData
{
    public class Normtime
    {
        public int Id;
        public float PlanOutQtyMakeReady = 0;
        public float PlanOutQtyWork = 0;
        public float PlanNormtimeMakeReady = 0;
        public float PlanNormtimeWork = 0;

        public Normtime(int id)
        {
            this.Id = id;
            //this.Equip = equip;
        }
    }
}
