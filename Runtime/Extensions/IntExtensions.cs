using System.Collections.Generic;
using System.Linq;

namespace VG.Extensions
{
    public static class IntExtensions
    {
        /// <summary>/// Возвращает слова в падеже, зависимом от заданного числа </summary>
        /// <param name="number">Число от которого зависит выбранное слово</param>
        /// <param name="nominativ">Именительный падеж слова. Например "день"</param>
        /// <param name="genetiv">Родительный падеж слова. Например "дня"</param>
        /// <param name="plural">Множественное число слова. Например "дней"</param>
        /// <returns></returns>
        public static string GetDeclension(this int number, string nominativ, string genetiv, string plural)
        {
            number %= 100;
            if (number >= 11 && number <= 19) return plural;

            var i = number % 10;
            switch (i)
            {
                case 1:
                    return nominativ;
                case 2:
                case 3:
                case 4:
                    return genetiv;
                default:
                    return plural;
            }
        }

        public static int GreatestCommonDivisor(params int[] numbers) => numbers.Aggregate(GreatestCommonDivisor);

        public static int GreatestCommonDivisor(this IEnumerable<int> numbers) =>
            numbers.Aggregate(GreatestCommonDivisor);

        public static int GreatestCommonDivisor(int x, int y) => y == 0 ? x : GreatestCommonDivisor(y, x % y);
    }
}