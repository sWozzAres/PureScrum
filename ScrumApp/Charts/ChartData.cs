using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScrumApp.Charts;

/// <summary>
/// JSON converter for ChartJS. Removes nulls from the serialized data because
/// in ChartJS 'null' has different behaviour to 'undefined'.
/// </summary>
public class ChartConverter : JsonConverter<Chart>
{
    public override Chart? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, Chart value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        if (value.Type is not null)
            writer.WriteString("type", value.Type);

        if (value.Data is not null)
        {
            writer.WriteStartObject("data");
            if (value.Data.Labels is not null)
            {
                writer.WriteStartArray("labels");
                foreach (var label in value.Data.Labels)
                {
                    writer.WriteStringValue(label);
                }
                writer.WriteEndArray();
            }

            if (value.Data.Datasets is not null)
            {
                writer.WriteStartArray("datasets");

                foreach (var dataset in value.Data.Datasets)
                {
                    writer.WriteStartObject();

                    if (dataset.Label is not null)
                        writer.WriteString("label", dataset.Label);
                    if (dataset.Data is not null)
                    {
                        writer.WriteStartArray("data");
                        foreach (var d in dataset.Data)
                        {
                            writer.WriteNumberValue(d);
                        }
                        writer.WriteEndArray();
                    }
                    if (dataset.BorderWidth is not null)
                        writer.WriteNumber("borderWidth", dataset.BorderWidth!.Value);
                    if (dataset.BackgroundColor is not null)
                    {
                        foreach (var b in dataset.BackgroundColor)
                        {
                            writer.WriteStringValue(b);
                        }
                    }
                    if (dataset.HoverOffset is not null)
                        writer.WriteNumber("hoverOffset", dataset.HoverOffset!.Value);
                    if (dataset.Fill is not null)
                        writer.WriteBoolean("fill", dataset.Fill!.Value);
                    writer.WriteEndObject();
                }


                writer.WriteEndArray();
            }
            writer.WriteEndObject();
        }

        if (value.Options is not null)
        {
            writer.WriteStartObject("options");

            if (value.Options.Scales is not null)
            {
                writer.WriteStartObject("scales");
                if (value.Options.Scales.Y is not null)
                {
                    WriteChartOptionsScalesAxis(value.Options.Scales.Y, "y");
                    //writer.WriteStartObject("y");
                    //if (value.Options.Scales.Y.BeginAtZero is not null)
                    //    writer.WriteBoolean("beginAtZero", value.Options.Scales.Y.BeginAtZero.Value);
                    //if (value.Options.Scales.Y.Stacked is not null)
                    //    writer.WriteBoolean("stacked", value.Options.Scales.Y.Stacked.Value);
                    //writer.WriteEndObject();
                }
                if (value.Options.Scales.X is not null)
                {
                    WriteChartOptionsScalesAxis(value.Options.Scales.X, "x");
                }
                writer.WriteEndObject();
            }

            if (value.Options.Responsive is not null)
                writer.WriteBoolean("responsive", value.Options.Responsive.Value);
            if (value.Options.MaintainAspectRatio is not null)
                writer.WriteBoolean("maintainAspectRatio", value.Options.MaintainAspectRatio.Value);
            if (value.Options.AspectRatio is not null)
                writer.WriteNumber("aspectRatio", value.Options.AspectRatio.Value);
            if (value.Options.ResizeDelay is not null)
                writer.WriteNumber("resizeDelay", value.Options.ResizeDelay.Value);

            writer.WriteEndObject();
        }

        writer.WriteEndObject();

        void WriteChartOptionsScalesAxis(ChartOptions.ChartOptionsScales.ChartOptionsScalesAxis axis, string identifier)
        {
            writer.WriteStartObject("y");
            if (axis.BeginAtZero is not null)
                writer.WriteBoolean("beginAtZero", axis.BeginAtZero.Value);
            if (axis.Stacked is not null)
                writer.WriteBoolean("stacked", axis.Stacked.Value);
            if (axis.Type is not null)
                writer.WriteString("type", axis.Type);

            if (axis.Time is not null)
            {
                writer.WriteStartObject("time");
                if (axis.Time.Unit is not null)
                    writer.WriteString("type", axis.Time.Unit);
                if (axis.Time.Source is not null)
                    writer.WriteString("type", axis.Time.Source);
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }
    }
}

[JsonConverter(typeof(ChartConverter))]
public class Chart
{
    public string? Type { get; set; }
    public ChartData? Data { get; set; }
    public ChartOptions? Options { get; set; }
}

public class ChartData
{
    public string[]? Labels { get; set; }
    public DataSets[]? Datasets { get; set; }

    public class DataSets
    {
        public string? Label { get; set; }
        public double[]? Data { get; set; }
        public int? BorderWidth { get; set; }
        public string[]? BackgroundColor { get; set; }
        public int? HoverOffset { get; set; }
        public bool? Fill { get; set; }
    }
}

public class ChartOptions
{
    public ChartOptionsScales? Scales { get; set; }
    public bool? Responsive { get; set; }
    public bool? MaintainAspectRatio { get; set; }
    public double? AspectRatio { get; set; }
    public int? ResizeDelay { get; set; }
    public class ChartOptionsScales
    {
        public ChartOptionsScalesAxis? Y { get; set; }
        public ChartOptionsScalesAxis? X { get; set; }
        public class ChartOptionsScalesAxis
        {
            public bool? BeginAtZero { get; set; }
            public bool? Stacked { get; set; }
            public string? Type { get; set; }
            public TimeAxis? Time { get; set; }
        }
    }
}
/// <summary>
/// https://www.chartjs.org/docs/4.4.0/axes/cartesian/time.html
/// </summary>
public class TimeAxis
{
    public string? Source { get; set; }
    //TODO public object? DisplayFormats { get; set; }
    public string? Unit { get; set; }
}
