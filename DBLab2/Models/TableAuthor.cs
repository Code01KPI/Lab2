
namespace DBLab2.Models
{
    internal class TableAuthor
    {
        public int author_id;

        public string? full_name;

        public string? country_od_origin;

        public TableAuthor(int id, string? name, string country)
        {
            this.author_id = id;
            full_name = name;
            country_od_origin = country;
        }

        public void Deconstructor(out int id)
        {
            id = this.author_id;
        }
    }
}
