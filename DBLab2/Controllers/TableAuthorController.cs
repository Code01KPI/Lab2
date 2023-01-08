using DBLab2.Models;

namespace DBLab2.Controllers
{
    internal class TableAuthorController : DataBaseController
    {
        public List<TableAuthor> authorList = new List<TableAuthor> ();

        public TableAuthorController() { }

        public override async Task InsertDataTableAsync()
        {
            foreach (var author in authorList)
            {
                await using var command = dataBase.DataSource.CreateCommand($"INSERT INTO public.\"Author\"(author_id, full_name, country_of_origin) VALUES({author.author_id}, '{author.full_name}', '{author.country_od_origin}')");
                var execute = await command.ExecuteNonQueryAsync();
            }
        }

        public override async Task UpdateDataTableAsync(int id)
        {
            await using var command = dataBase.DataSource.CreateCommand($"UPDATE public.\"Author\" SET author_id = {author?.author_id}, full_name = '{author?.full_name}', country_of_origin = '{author?.country_od_origin}' WHERE author_id = {id}");
            var execute = await command.ExecuteNonQueryAsync();
        }

        public override async Task DeleteDataTableAsync(int id)
        {
            await using var command1 = dataBase.DataSource.CreateCommand($"DELETE FROM public.\"Author_Book\" WHERE author_fk = {id}");
            var execute1 = await command1.ExecuteNonQueryAsync();

            await using var command2 = dataBase.DataSource.CreateCommand($"DELETE FROM public.\"Author\" WHERE author_id = {id}");
            var execute2 = await command2.ExecuteNonQueryAsync();
        }

        public override async Task GenerateDataTableAsync(int amountOfData)
        {
            int newFirstId = FindMaxPKAsync("Author", "author_id").Result + 1;
            await PrintCountOfRowAsync("Author");
            await using var command = dataBase.DataSource.CreateCommand($"INSERT INTO public.\"Author\"(author_id, full_name, country_of_origin) " +
                $"SELECT generate_series({newFirstId},{amountOfData + newFirstId - 1}), md5(random()::text), md5(random()::text)");
            var execute = await command.ExecuteNonQueryAsync();
            await PrintCountOfRowAsync("Author");
        }
    }
}
