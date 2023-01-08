using DBLab2.Models;
using Npgsql;

namespace DBLab2.Controllers
{
    internal class TableAuthorBookController : DataBaseController
    {
        public List<TableAuthorBook> authorBookList = new List<TableAuthorBook>();

        public TableAuthorBookController() { }

        public override async Task InsertDataTableAsync()
        {
            foreach (var authorBook in authorBookList)
            {
                await using var command = dataBase.DataSource?.CreateCommand($"INSERT INTO public.\"Author_Book\"(author_book_id, book_fk, author_fk) VALUES({authorBook.author_book_id}, '{authorBook.book_fk}', '{authorBook.author_fk}')");
                var execute = await command.ExecuteNonQueryAsync();
            }
        }

        public override async Task UpdateDataTableAsync(int id) 
        {
            await using var command = dataBase.DataSource.CreateCommand($"UPDATE public.\"Author_Book\" SET author_book_id = {authorBook?.author_book_id}, book_fk = {authorBook?.book_fk}, author_fk = {authorBook.author_fk} WHERE author_book_id = {id}");
            var execute = await command.ExecuteNonQueryAsync();
        }

        public override async Task DeleteDataTableAsync(int id)
        {
            await using var command = dataBase.DataSource.CreateCommand($"DELETE FROM public.\"Author_Book\" WHERE author_book_id = {id}");
            var execute = await command.ExecuteNonQueryAsync();
        }

        public override async Task GenerateDataTableAsync(int amountOfData)
        {
            if (GetCountOfRow("Author").Result > 0 && GetCountOfRow("Book").Result > 0)
            {
                int newFirstId = FindMaxPKAsync("Author_Book", "author_book_id").Result + 1;
                await PrintCountOfRowAsync("Author_Book");
                await using var command = dataBase.DataSource.CreateCommand($"INSERT INTO public.\"Author_Book\"(author_book_id, book_fk, author_fk) " +
                    $"SELECT generate_series({newFirstId},{amountOfData + newFirstId - 1}), book_id, author_id FROM public.\"Book\", public.\"Author\", generate_series({newFirstId},{amountOfData + newFirstId - 1}) limit {amountOfData}");
                var execute = await command.ExecuteNonQueryAsync();
                await PrintCountOfRowAsync("Author_Book");
            }
            else
                throw new NpgsqlException("Parents tables are empty!");
            }
    }
}

