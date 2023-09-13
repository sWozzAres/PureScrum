namespace ScrumApi
{
    public partial class ProductShort
    {
        public bool Checked { get; set; } = false;
        public ProductShort(string id, string name) => (Id, Name) = (id, name);
    }

    public partial class ProductBacklogItem
    {
        public bool Checked { get; set; } = false;
        public ProductBacklogItem(string id, string name, int status, string sprintName, string productName, float estimationPoints)
            => (Id, Name, Status, SprintName, ProductName, EstimationPoints) = (id, name, (PbiStatus)status, sprintName, productName, estimationPoints);
    }

    public partial class SprintShort
    {
        public bool Checked { get; set; } = false;
        public SprintShort(string id, string name)
        {
            Id = id;
            Name = name;

            ExpectedDeliveryDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(DateTimeOffset.MaxValue);
            ExpectedDeliveryDateIsValid = false;
            Status = 0;
        }
    }
}