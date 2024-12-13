namespace TechChallenge.Controllers.Contacts.Dto
{
    public class ContactsGetDto
    {
        public List<ContactGetDto> Contacts { get; set; }
        public RegionGetDto Region { get; set; }
    }


    public class RegionGetDto
    {
        public Guid Id { get; set; }
        public string DDD { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }

    public class ContactGetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    
        public string CellphoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Active { get; set; }
    }
}
