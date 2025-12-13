namespace FuelMaster.HeadOffice.Core.Helpers
{
    public class DateTimeCulture
    {
        public static DateTime Now
        {
            get
            {
                var _serverTime = DateTime.Now;
                var _localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(_serverTime, TimeZoneInfo.Local.Id, "Syria Standard Time");

                return _localTime;
            }
        }
        public static DateTime Today
        {
            get
            {
                var _serverToday = DateTime.Today;
                var _localToday = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(_serverToday, TimeZoneInfo.Local.Id, "Syria Standard Time");

                return _localToday;
            }
        }
    }
}
