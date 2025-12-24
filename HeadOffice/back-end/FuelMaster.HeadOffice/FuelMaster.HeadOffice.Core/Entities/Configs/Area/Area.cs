namespace FuelMaster.HeadOffice.Core.Entities.Configs.Area;

public class Area : Entity<int>
{
    public string ArabicName { get; private set; }
    public string EnglishName { get; private set; }
    public int StationCount { get; private set; }
    protected List<Station> _stations = new List<Station>();

    public City? City { get; private set; }
    public int CityId { get; private set; }
    public IReadOnlyList<Station> Stations => _stations.AsReadOnly();

    public Area(string arabicName, string englishName)
    {
        ArabicName = arabicName;
        EnglishName = englishName;
    }

    public void Update(string arabicName, string englishName)
    {
        ArabicName = arabicName;
        EnglishName = englishName;
    }

    public void AddStation(Station station)
    {
        _stations.Add(station);
        StationCount++;
    }
}