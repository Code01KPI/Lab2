

namespace DBLab2.Models
{ 
    internal class TableBook
    {
        public int book_id;

        public int date_of_publication;

        public int number_of_pages;

        public string? genre;

        public int bk_library_id;

        public string? book_name;

        public TableBook(int id, int date, int numberOfPages, string gen, int libraryId, string name)
        {
            this.book_id = id;
            date_of_publication = date;
            number_of_pages = numberOfPages;
            genre = gen;
            bk_library_id = libraryId;
            book_name = name;
        }

        public void Deconstructor(out int id)
        {
            id = this.book_id;
        }
    }
}
