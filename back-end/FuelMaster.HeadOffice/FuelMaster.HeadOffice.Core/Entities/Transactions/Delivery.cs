namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Delivery : Entity<int>
    {
        public string Transport { get; private set; }
        public string InvoiceNumber { get; private set; }

        // The volume that should be received.
        public decimal PaidVolume { get; private set; }

        // The real volume that was received.
        public decimal RecivedVolume { get; private set; }

        // In meter/centimeter
        public decimal TankOldLevel { get; private set; }
        public decimal TankNewLevel { get; private set; }

        // In liter
        public decimal TankOldVolume { get; private set; }
        public decimal TankNewVolume { get; private set; }

        // GL (Gain/Loss) = RecivedVolume - PaidVolume
        public decimal GL { get; private set; }
        public Tank? Tank { get; private set; }
        public int TankId { get; private set; }

        internal Delivery(
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
            if (paidVolume <= 0) throw new ArgumentException("PaidVolume must be greater than 0");
            if (recivedVolume <= 0) throw new ArgumentException("RecivedVolume must be greater than 0");
            if (tankOldLevel <= 0) throw new ArgumentException("TankOldLevel must be greater than 0");
            if (tankOldVolume <= 0) throw new ArgumentException("TankOldVolume must be greater than 0");
            if (tankNewLevel <= 0) throw new ArgumentException("TankNewLevel must be greater than 0");
            if (tankNewVolume <= 0) throw new ArgumentException("TankNewVolume must be greater than 0");
            if (gL <= 0) throw new ArgumentException("GL must be greater than 0");
            if (tankId <= 0) throw new ArgumentException("TankId must be greater than 0");

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
