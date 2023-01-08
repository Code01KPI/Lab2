using Npgsql;
using DBLab2.Models;
using System.Diagnostics;

namespace DBLab2.Controllers
{
    /// <summary>
    /// Логіка об'єкта БД.
    /// </summary>
    internal class DataBaseController
    {
        public DataBase dataBase;

        public TableAuthor? author { get; set; }

        public TableAuthorBook? authorBook { get; set; }

        public TableBook? book { get; set; }

        public TableLibrary? library { get; set; }

        public TableReader? reader { get; set; }

        public TablePerson? person { get; set; }

        public DataBaseController()
        {
            try
            {
                dataBase = new DataBase("Host=localhost;Port=5432;Username=postgres;Password=1mn2487rt;Database=School;");

            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine(argEx.Message);   
                Console.WriteLine("Fix connection string!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task InsertDataAsync(string tableName, DataBaseController tableControler)
        {
            int beforeRows;
            try
            {
                beforeRows = await PrintCountOfRowAsync(tableName);
                await tableControler.InsertDataTableAsync();
                Console.WriteLine($"Rows affected - {await PrintCountOfRowAsync(tableName) - beforeRows}");
            }
            catch (NpgsqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
            }
        }

        public async Task UpdateDataAsync(string tableName, string columnIdName, int id, DataBaseController tableControler)
        {
            try
            {
                await SelectOneRow(id, tableName, columnIdName);
                await tableControler.UpdateDataTableAsync(id);
                await SelectOneRow(id, tableName, columnIdName);
            }
            catch (NpgsqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
            }
        }

        public async Task DeleteDataAsync(string tableName, int id, DataBaseController tableControler)
        {
            int beforeRows;
            try
            {
                beforeRows = await PrintCountOfRowAsync(tableName);
                await tableControler.DeleteDataTableAsync(id);
                Console.WriteLine($"Rows affected - {beforeRows - await PrintCountOfRowAsync(tableName)}");
            }
            catch (NpgsqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
            }
        }

        public virtual async Task InsertDataTableAsync() { }

        public virtual async Task UpdateDataTableAsync(int id) { }

        public virtual async Task DeleteDataTableAsync(int id) { }

        public virtual async Task GenerateDataTableAsync(int amountOfData) { }

        public async Task SearchAsync(int bk_library_id, string? bookNameLike)
        {
            Stopwatch swatch = new Stopwatch();
            try 
            {
                swatch.Start();
                await using var command = dataBase.DataSource.CreateCommand($"SELECT B.book_name, A.full_name FROM public.\"Book\" as B, public.\"Author\" as A, public.\"Author_Book\" as AB WHERE AB.book_fk = B.book_id AND AB.author_fk = A.author_id AND B.bk_library_id = {bk_library_id} AND B.book_name LIKE '{bookNameLike}%'");
                var reader = await command.ExecuteReaderAsync();
                swatch.Stop();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (i == reader.FieldCount - 1)
                            {
                                Console.Write(reader.GetValue(i));
                                break;
                            }

                            Console.Write($"{reader.GetValue(i)} - ");
                        }
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No data found!");
                }
            }
            catch (NpgsqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
            }
            Console.WriteLine($"Time: {swatch.ElapsedMilliseconds} ms");
        }

        public async Task SearchAsync(string? bookNameLike, string? genreLike)
        {
            Stopwatch swatch = new Stopwatch();
            try
            {
                swatch.Start();
                await using var command = dataBase.DataSource.CreateCommand($"SELECT B.book_name, B.genre, L.giving_time FROM public.\"Book\" as B, public.\"Library\" as L WHERE book_name LIKE '{bookNameLike}%' AND bk_library_id = id AND genre LIKE '{genreLike}%'");
                var reader = await command.ExecuteReaderAsync();
                swatch.Stop();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (i == reader.FieldCount - 1)
                            {
                                Console.Write(reader.GetValue(i));
                                break;
                            }

                            Console.Write($"{reader.GetValue(i)} - ");
                        }
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No data found!");
                }
            }
            catch (NpgsqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
            }
            Console.WriteLine($"Time: {swatch.ElapsedMilliseconds} ms");
        }

        public async Task SearchAsync(string? fullNameLike, bool is_have_ticket)
        {
            Stopwatch swatch = new Stopwatch();
            try
            {
                swatch.Start();
                await using var command = dataBase.DataSource.CreateCommand($"SELECT P.full_name, L.giving_time FROM public.\"Person\" as P, public.\"Library\" as L, public.\"Reader\" as R WHERE full_name LIKE '{fullNameLike}%' AND R.library_id = L.id AND R.person_id = P.person_id AND is_have_ticket = {is_have_ticket}");
                var reader = await command.ExecuteReaderAsync();
                swatch.Stop();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (i == reader.FieldCount - 1)
                            {
                                Console.Write(reader.GetValue(i));
                                break;
                            }

                            Console.Write($"{reader.GetValue(i)} - ");
                        }
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No data found!");
                }
            }
            catch (NpgsqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
            }
            Console.WriteLine($"Time: {swatch.ElapsedMilliseconds} ms");
        }

        /// <summary>
        /// Виводить на консоль кількість рядків в умовній таблиці.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<int> PrintCountOfRowAsync(string tableName)
        {
            int count = 0;
            await using var command = dataBase?.DataSource?.CreateCommand($"SELECT COUNT(*) FROM public.\"{tableName}\"");
            await using var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"Table {tableName} has {reader.GetInt32(0)} rows");
                    count = reader.GetInt32(0);
                }
            }
            else
            {
                Console.WriteLine("Empty table!");
            }

            return count;
        }

        /// <summary>
        /// Повертає кількість рядків в умовній таблиці.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<int> GetCountOfRow(string tableName)
        {
            int count = 0;
            await using var command = dataBase?.DataSource?.CreateCommand($"SELECT COUNT(*) FROM public.\"{tableName}\"");
            await using var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                    count = reader.GetInt32(0);
            }

            return count;
        }

        /// <summary>
        /// Виводить на консоль потрібний рядок БД.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task SelectOneRow(long id, string tableName, string columnName)
        {
            object[] arrayOfValues = new object[] {};
            await using var command = dataBase.DataSource.CreateCommand($"SELECT * FROM public.\"{tableName}\" WHERE {columnName} = {id}");
            await using var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (i == reader.FieldCount - 1)
                        {
                            Console.Write(reader.GetValue(i));
                            break;
                        }

                        Console.Write($"{reader.GetValue(i)} - ");
                    }
                    Console.WriteLine();
                }
            }
            else
                Console.WriteLine("Table is empty!");
        }

        /// <summary>
        /// Перевіряє чи є потрібні дані в батьківській таблиці.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsTableHaveTheRow(int id, string tableName, string columnName)
        {
            await using var command = dataBase.DataSource.CreateCommand($"SELECT * FROM public.\"{tableName}\" WHERE {columnName} = {id}");
            await using var reader = await command.ExecuteReaderAsync();
            return reader.HasRows;
        }
        
        /// <summary>
        /// Перевіряє чи дублюється PK.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public async Task<bool> IsPKDublicated(int id, string tableName, string columnName)
        {
            await using var command = dataBase.DataSource.CreateCommand($"SELECT * FROM public.\"{tableName}\" WHERE {columnName} = {id}");
            await using var reader = await command.ExecuteReaderAsync();
            return reader.HasRows;
        }

        /// <summary>
        /// Метод для знаходження найбільшого значення PK.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public async Task<int> FindMaxPKAsync(string tableName, string columnName)
        {
            int maxId = 0;
            await using var command = dataBase.DataSource.CreateCommand($"SELECT {columnName} FROM public.\"{tableName}\" ORDER BY {columnName} DESC limit 1");
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                if (reader.HasRows)
                    maxId = reader.GetInt32(0);
                else
                    throw new NpgsqlException("Table is empty!");
            }
            return maxId;
        }


    }
}
