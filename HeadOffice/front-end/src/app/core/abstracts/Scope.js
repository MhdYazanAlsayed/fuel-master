export class Scope {
  /// <summary>
  /// This employee should see the reports over whole company, maybe it's CEO or something like that.
  /// </summary>
  static ALL = 0;
  /// <summary>
  /// This employee should see the reports over a city, maybe it's a city manager or something like that.
  /// </summary>
  static City = 1;
  /// <summary>
  /// This employee should see the reports over an area, maybe it's an area manager or something like that.
  /// </summary>
  static Area = 2;
  /// <summary>
  /// This employee should see the reports over a station, maybe it's a station manager or something like that.
  /// </summary>
  static Station = 3;
  /// <summary>
  /// This employee should see the reports over himself, maybe it's a normal employee or something like that.
  /// </summary>
  static Self = 4;
}
