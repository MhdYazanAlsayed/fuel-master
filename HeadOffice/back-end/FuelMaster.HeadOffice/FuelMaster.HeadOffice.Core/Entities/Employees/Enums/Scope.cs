public enum Scope 
{
    /// <summary>
    /// This employee should see the reports over whole company, maybe it's CEO or something like that.
    /// </summary>
    ALL,
    /// <summary>
    /// This employee should see the reports over a city, maybe it's a city manager or something like that.
    /// </summary>
    City,
    /// <summary>
    /// This employee should see the reports over an area, maybe it's an area manager or something like that.
    /// </summary>
    Area,
    /// <summary>
    /// This employee should see the reports over a station, maybe it's a station manager or something like that.
    /// </summary>
    Station,
    /// <summary>
    /// This employee should see the reports over himself, maybe it's a normal employee or something like that.
    /// </summary>
    Self
}