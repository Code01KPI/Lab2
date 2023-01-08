using DBLab2.Models;
using Npgsql;

namespace DBLab2.Controllers
{
    internal class TableBookController : DataBaseController
    {
        public List<TableBook> bookList = new List<TableBook>();

        public TableBookController() { }

        public override async Task InsertDataTableAsync()
        {
            foreach (var book in bookList)
            {
                await using var command = dataBase.DataSource.CreateCommand($"INSERT INTO public.\"Book\"(book_id, date_of_publication, number_of_pages, genre, bk_library_id, book_name)" +
                    $" VALUES({book.book_id}, {book.date_of_publication}, {book.number_of_pages}, '{book.genre}', {book.bk_library_id}, '{book.book_name}')");
                var execute = await command.ExecuteNonQueryAsync();
            }
        }

        public override async Task UpdateDataTableAsync(int id)
        {
            await using var command = dataBase.DataSource.CreateCommand($"UPDATE public.\"Book\" SET book_id = {book?.book_id}, " +
                $"date_of_publication = {book?.date_of_publication}, number_of_pages = {book?.number_of_pages}, genre = '{book?.genre}', bk_library_id = {book?.bk_library_id}, book_name = '{book?.book_name}'" +
                $" WHERE book_id = {id}");
            var execute = await command.ExecuteNonQueryAsync();
        }

        
        public override async Task DeleteDataTableAsync(int id)
        {
            await using var command1 = dataBase.DataSource.CreateCommand($"DELETE FROM public.\"Author_Book\" WHERE book_fk = {id}");
            var execute1 = await command1.ExecuteNonQueryAsync();

            await using var command2 = dataBase.DataSource.CreateCommand($"DELETE FROM public.\"Book\" WHERE book_id = {id}");
            var execute2 = await command2.ExecuteNonQueryAsync();
        }

        public override async Task GenerateDataTableAsync(int amountOfData)
        {
            if (GetCountOfRow("Library").Result > 0)
            {
                int newFirstId = FindMaxPKAsync("Book", "Book_id").Result + 1;
                await PrintCountOfRowAsync("Book");
                await using var command = dataBase.DataSource.CreateCommand($"INSERT INTO public.\"Book\"(book_id, genre, book_name, date_of_publication, number_of_pages, bk_library_id) SELECT generate_series({newFirstId},{amountOfData + newFirstId - 1}), md5(random()::text), md5(random()::text), trunc(random() * 2023)::int, trunc(random() * 200)::int, id FROM generate_series(1, {amountOfData}), public.\"Library\" limit {amountOfData}");
                var execute = await command.ExecuteNonQueryAsync();
                await PrintCountOfRowAsync("Book");
            }
            else
                throw new NpgsqlException("Parent table is empty!");
        }
    }
}
