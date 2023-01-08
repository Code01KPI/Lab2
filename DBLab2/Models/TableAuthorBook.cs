
namespace DBLab2.Models
{
    internal class TableAuthorBook
    {
        public int author_book_id;

        public int book_fk;

        public int author_fk;

        public TableAuthorBook(int id, int b_fk, int a_fk)
        {
            this.author_book_id = id;
            book_fk = b_fk;
            author_fk = a_fk;
        }

        public void Deconstructor(out int id)
        {
            id = this.author_book_id;
        }
    }
}
