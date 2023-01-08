using DBLab2.Models;
using Npgsql;

namespace DBLab2.Controllers
{
    internal class TableReaderController : DataBaseController
    {
        public List<TableReader> readerList = new List<TableReader>();

        public TableReaderController() { }

        public override async Task InsertDataTableAsync()
        {
            foreach (var reader in readerList)
            {
                await using var command = dataBase.DataSource?.CreateCommand($"INSERT INTO public.\"Reader\"(id, library_id, person_id, taken_book) VALUES({reader.id}, {reader.library_id}, {reader.person_id}, '{reader.taken_book}')");
                var execute = await command.ExecuteNonQueryAsync();
            }
        }

        
        public override async Task UpdateDataTableAsync(int id)
        {
            await using var command = dataBase.DataSource.CreateCommand($"UPDATE public.\"Reader\" SET id = {reader?.id}, library_id = {reader?.library_id}, person_id = {reader?.person_id}, taken_book = '{reader?.taken_book}' WHERE id = {id}");
            var execute = await command.ExecuteNonQueryAsync();
        }
        
        public override async Task DeleteDataTableAsync(int id)
        {
            await using var command = dataBase.DataSource.CreateCommand($"DELETE FROM public.\"Reader\" WHERE id = {id}");
            var execute = await command.ExecuteNonQueryAsync();
        }

        public override async Task GenerateDataTableAsync(int amountOfData)
        {
            if (GetCountOfRow("Library").Result > 0 || GetCountOfRow("Person").Result > 0)
            {
                int newFirstId = FindMaxPKAsync("Reader", "id").Result + 1;
                await PrintCountOfRowAsync("Reader");
                await using var command = dataBase.DataSource.CreateCommand($"INSERT INTO public.\"Reader\"(id, taken_book, library_id, person_id) " +
                    $"SELECT generate_series({newFirstId},{amountOfData + newFirstId - 1}), md5(random()::text), id, person_id FROM public.\"Library\", public.\"Person\" limit {amountOfData}");
                var execute = await command.ExecuteNonQueryAsync();
                await PrintCountOfRowAsync("Reader");
            }
            else
                throw new NpgsqlException("Parents tables are empty!");
        }
    }


}
