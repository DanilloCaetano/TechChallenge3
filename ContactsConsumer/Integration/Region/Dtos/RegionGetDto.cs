namespace Integration.Region.Dtos
{
    public class RegionRequestGetDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string DDD { get; set; }

        public bool Active { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
