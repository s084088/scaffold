using Newtonsoft.Json;

using System.Globalization;

namespace ApiServer.Comm;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string DateFormat = "yyyy-MM-dd";

    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return DateOnly.ParseExact((string)reader.Value, DateFormat, CultureInfo.InvariantCulture);
    }

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
}

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private const string TimeFormat = "HH:mm:ss.FFFFFFF";

    public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return TimeOnly.ParseExact((string)reader.Value, TimeFormat, CultureInfo.InvariantCulture);
    }

    public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(TimeFormat, CultureInfo.InvariantCulture));
    }
}

public class DateOnlyCanNullJsonConverter : JsonConverter<DateOnly?>
{
    private const string DateFormat = "yyyy-MM-dd";

    public override DateOnly? ReadJson(JsonReader reader, Type objectType, DateOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (string.IsNullOrWhiteSpace(reader.ToString())) return null;
        return DateOnly.ParseExact((string)reader.Value, DateFormat, CultureInfo.InvariantCulture);
    }

    public override void WriteJson(JsonWriter writer, DateOnly? value, JsonSerializer serializer)
    {
        if (value == null)
            writer.WriteValue((string)null);
        else
            writer.WriteValue(value.Value.ToString(DateFormat, CultureInfo.InvariantCulture));

    }
}

public class TimeOnlyCanNullJsonConverter : JsonConverter<TimeOnly?>
{
    private const string TimeFormat = "HH:mm:ss.FFFFFFF";

    public override TimeOnly? ReadJson(JsonReader reader, Type objectType, TimeOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (string.IsNullOrWhiteSpace(reader.ToString())) return null;
        return TimeOnly.ParseExact((string)reader.Value, TimeFormat, CultureInfo.InvariantCulture);
    }

    public override void WriteJson(JsonWriter writer, TimeOnly? value, JsonSerializer serializer)
    {
        if (value == null)
            writer.WriteValue((string)null);
        else
            writer.WriteValue(value.Value.ToString(TimeFormat, CultureInfo.InvariantCulture));
    }
}