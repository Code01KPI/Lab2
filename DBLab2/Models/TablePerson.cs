
namespace DBLab2.Models
{
    internal class TablePerson
    {
        public int person_id;

        public string? full_name;

        public bool is_have_ticket;

        public TablePerson(int id, string? fullName, bool isHaveTicket)
        {
            person_id = id;
            full_name = fullName;
            is_have_ticket = isHaveTicket;
        }
    }
}
