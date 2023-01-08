
namespace DBLab2.Models
{
    internal class TableReader
    {
        public int id;

        public int library_id;

        public int person_id;

        public string? taken_book;

        public TableReader(int id, int libraryId, int personId, string? takenBook)
        {
            this.id = id;
            library_id = libraryId;
            person_id = personId;
            taken_book = takenBook;
        }
    }
}
