

namespace DBLab2.Models
{
    internal class TableLibrary
    {
        public int id;

        public DateOnly? giving_time;

        public DateOnly? return_time;

        public DateOnly? actual_return_time;

        public TableLibrary (int id, DateOnly? givingTime, DateOnly? returnTime, DateOnly? actualReturnTime)
        {
            this.id = id;
            giving_time = givingTime;
            return_time = returnTime;
            actual_return_time = actualReturnTime;
        }

        /// <summary>
        /// Створення об'єкту класа DateOnly на основі введених користовачем значень.
        /// </summary>
        /// <param name="yyyy"></param>
        /// <param name="mm"></param>
        /// <param name="dd"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static DateOnly createDate(int yyyy, int mm, int dd)
        {
            if (yyyy < 1 || mm < 1 || dd < 1)
                throw new ArgumentException("It's incorrect value of date!");
            else if (mm > 12 || dd > 31)
                throw new ArgumentException("It's incorrect value of date!");

            return new DateOnly(yyyy, mm, dd);
        }
    }
}
