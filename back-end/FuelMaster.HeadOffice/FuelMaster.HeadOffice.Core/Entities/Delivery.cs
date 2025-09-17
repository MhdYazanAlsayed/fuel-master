namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Delivery : EntityBase<int>
    {
        public string Transport { get; set; }
        public string InvoiceNumber { get; set; }

        // The volume that should be received.
        public decimal PaidVolume { get; set; }

        // The real volume that was received.
        public decimal RecivedVolume { get; set; }

        // In meter/centimeter
        public decimal TankOldLevel { get; set; }
        public decimal TankNewLevel { get; set; }

        // In liter
        public decimal TankOldVolume { get; set; }
        public decimal TankNewVolume { get; set; }

        // GL (Gain/Loss) = RecivedVolume - PaidVolume
        public decimal GL { get; set; }
        public Tank? Tank { get; set; }
        public int TankId { get; set; }

        private Delivery(
            string transport, 
            string invoiceNumber, 
            decimal paidVolume, 
            decimal recivedVolume,
            decimal tankOldLevel, 
            decimal tankNewLevel, 
            decimal tankOldVolume, 
            decimal tankNewVolume,
            decimal gL, 
            int tankId)
        {
            Transport = transport;
            InvoiceNumber = invoiceNumber;
            PaidVolume = paidVolume;
            RecivedVolume = recivedVolume;
            TankOldLevel = tankOldLevel;
            TankNewLevel = tankNewLevel;
            TankOldVolume = tankOldVolume;
            TankNewVolume = tankNewVolume;
            GL = gL;
            TankId = tankId;
        }

        public static Delivery Create (string transport, string invoiceNumber, decimal paidVolume, decimal recivedVolume, int tankId, decimal tankOldLevel, decimal tankOldVolume)
        {
            return new Delivery(
                transport , 
                invoiceNumber , 
                paidVolume , 
                recivedVolume , 
                tankOldLevel , 
                tankOldLevel , 
                tankOldVolume, 
                tankOldVolume + recivedVolume, 
                recivedVolume - paidVolume , 
                tankId);
        }
    }
}
