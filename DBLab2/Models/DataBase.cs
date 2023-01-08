using Npgsql;

namespace DBLab2.Models
{
    /// <summary>
    /// Основний клс для роботи з БД(створення/підключення/робота з данними).
    /// </summary>
    internal class DataBase
    {
        private string? ConnectionString;

        public NpgsqlDataSource? DataSource { get; set; }

        public DataBase(string? connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string is invalid!");
            
            ConnectionString = connectionString;
            ConnectToDBAsync();
        }



        /// <summary>
        /// Підключення до бази даних.
        /// </summary>
        private void ConnectToDBAsync() => DataSource = NpgsqlDataSource.Create(ConnectionString);
    }
}