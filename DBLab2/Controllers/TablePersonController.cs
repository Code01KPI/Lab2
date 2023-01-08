using DBLab2.Models;

namespace DBLab2.Controllers
{
    internal class TablePersonController : DataBaseController
    {
        public List<TablePerson> personList = new List<TablePerson>();

        public TablePersonController() { }

        public override async Task InsertDataTableAsync()
        {
            foreach (var person in personList)
            {
                await using var command = dataBase.DataSource?.CreateCommand($"INSERT INTO public.\"Person\"(person_id, full_name, is_have_ticket) VALUES({person.person_id}, '{person.full_name}', {person.is_have_ticket})");
                var execute = await command.ExecuteNonQueryAsync();
            }
        }
        
        
        public override async Task UpdateDataTableAsync(int id)
        {
            await using var command = dataBase.DataSource.CreateCommand($"UPDATE public.\"Person\" SET person_id = {person?.person_id}, full_name = '{person?.full_name}', is_have_ticket = {person?.is_have_ticket} WHERE person_id = {id}");
            var execute = await command.ExecuteNonQueryAsync();
        }

        public override async Task DeleteDataTableAsync(int id)
        {
            await using var command1 = dataBase.DataSource.CreateCommand($"DELETE FROM public.\"Reader\" WHERE person_id = {id}");
            var execute1 = await command1.ExecuteNonQueryAsync();

            await using var command2 = dataBase.DataSource.CreateCommand($"DELETE FROM public.\"Person\" WHERE person_id = {id}");
            var execute2 = await command2.ExecuteNonQueryAsync();
        }

        public override async Task GenerateDataTableAsync(int amountOfData)
        {
            int newFirstId = FindMaxPKAsync("Person", "person_id").Result + 1;
            await PrintCountOfRowAsync("Person");
            await using var command = dataBase.DataSource.CreateCommand($"INSERT INTO public.\"Person\"(person_id, full_name, is_have_ticket) " +
                $"SELECT generate_series({newFirstId},{amountOfData + newFirstId - 1}), md5(random()::text), random() > 0.5");
            var execute = await command.ExecuteNonQueryAsync();
            await PrintCountOfRowAsync("Person");
        }
    }
}
