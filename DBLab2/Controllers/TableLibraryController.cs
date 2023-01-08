using DBLab2.Models;

namespace DBLab2.Controllers
{

    internal class TableLibraryController : DataBaseController
    {
        public List<TableLibrary> libraryList = new List<TableLibrary>();

        public TableLibraryController() { }

        public override async Task InsertDataTableAsync()
        {
            foreach (var lib in libraryList)
            {
                await using var command = dataBase.DataSource.CreateCommand($"INSERT INTO public.\"Library\"(id, giving_time, return_time, actual_return_time)" +
                    $" VALUES({lib.id}, :giving_time, :return_time, :actual_return_time)");
                command.Parameters.AddWithValue(":giving_time", lib.giving_time);
                command.Parameters.AddWithValue(":return_time", lib.return_time);
                command.Parameters.AddWithValue(":actual_return_time", lib.actual_return_time);
                var execute = await command.ExecuteNonQueryAsync();
            }
        }

        
        public override async Task UpdateDataTableAsync(int id)
        {
            await using var command = dataBase.DataSource.CreateCommand($"UPDATE public.\"Library\" SET id = {library?.id}, " +
                $"giving_time = :giving_time, return_time = :return_time, actual_return_time = :actual_return_time" +
                $" WHERE id = {id}");
            command.Parameters.AddWithValue(":giving_time", library?.giving_time);
            command.Parameters.AddWithValue(":return_time", library?.return_time);
            command.Parameters.AddWithValue(":actual_return_time", library?.actual_return_time);
            var execute = await command.ExecuteNonQueryAsync();
        }

        
        public override async Task DeleteDataTableAsync(int id)
        {
            await using var command1 = dataBase.DataSource.CreateCommand($"DELETE FROM public.\"Author_Book\" WHERE book_fk = {id}");
            var execute1 = await command1.ExecuteNonQueryAsync();

            await using var command2 = dataBase.DataSource.CreateCommand($"DELETE FROM public.\"Book\" WHERE bk_library_id = {id}");
            var execute2 = await command2.ExecuteNonQueryAsync();

            await using var command3 = dataBase.DataSource.CreateCommand($"DELETE FROM public.\"Reader\" WHERE library_id = {id}");
            var execute3 = await command3.ExecuteNonQueryAsync();

            await using var command4 = dataBase.DataSource.CreateCommand($"DELETE FROM public.\"Library\" WHERE id = {id}");
            var execute4 = await command4.ExecuteNonQueryAsync();
        }

        public override async Task GenerateDataTableAsync(int amountOfData)
        {
            int newFirstId = FindMaxPKAsync("Library", "id").Result + 1;
            await PrintCountOfRowAsync("Library");
            await using var command = dataBase.DataSource.CreateCommand($"INSERT INTO public.\"Library\"(id, giving_time, return_time, actual_return_time) " +
                $"SELECT generate_series({newFirstId},{amountOfData + newFirstId - 1}), timestamp '2020-01-01 00:00:00' + random() * (timestamp '2021-01-01 00:00:00' - timestamp '2020-01-02 00:00:00')," +
                $"timestamp '2022-01-01 00:00:00' + random() * (timestamp '2023-01-01 00:00:00' - timestamp '2022-01-02 00:00:00'), timestamp '2021-01-01 00:00:00' + random() * (timestamp '2022-01-01 00:00:00' - timestamp '2021-01-02 00:00:00')");
            var execute = await command.ExecuteNonQueryAsync();
            await PrintCountOfRowAsync("Library");
        }
    }
}
