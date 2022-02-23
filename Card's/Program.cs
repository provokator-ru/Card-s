﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Cards;
using Extensions;

namespace Cards

{
    public static class Extensions

    {

        public static IEnumerable<T> InterleaveSequenceWith<T>

        (this IEnumerable<T> first, IEnumerable<T> second)

        {
            var firstIter = first.GetEnumerator();
            var secondIter = second.GetEnumerator();

            while (firstIter.MoveNext() && secondIter.MoveNext())
            {
                yield return firstIter.Current;
                yield return secondIter.Current;
            }
        }

        public static bool SequenceEquals<T>
    (this IEnumerable<T> first, IEnumerable<T> second)
        {
            var firstIter = first.GetEnumerator();
            var secondIter = second.GetEnumerator();

            while (firstIter.MoveNext() && secondIter.MoveNext())
            {
                if (!firstIter.Current.Equals(secondIter.Current))
                {
                    return false;
                }
            }

            return true;
        }
    }


}

namespace dev_rus
{
    class Program
    {
        static IEnumerable<string> Suits()
        {
            yield return "трефы";
            yield return "бубны";
            yield return "черви";
            yield return "пики";
        }

        static IEnumerable<string> Ranks()
        {
            yield return "2";
            yield return "3";
            yield return "4";
            yield return "5";
            yield return "6";
            yield return "7";
            yield return "8";
            yield return "9";
            yield return "10";
            yield return "Валет";
            yield return "Дама";
            yield return "Король";
            yield return "Туз";
        }
        // Program.cs

        public static void Main(string[] args)
        {
            var startingDeck = (from s in Suits().LogQuery("Suit Generation")
                                from r in Ranks().LogQuery("Value Generation")
                                select new { Suit = s, Rank = r })
                                .LogQuery("Starting Deck")
                                .ToArray();

            foreach (var c in startingDeck)
            {
                Console.WriteLine(c);
            }

            Console.WriteLine();

            var times = 0;
            var shuffle = startingDeck;

            do
            {
                /*
                shuffle = shuffle.Take(26)
                    .LogQuery("Top Half")
                    .InterleaveSequenceWith(shuffle.Skip(26).LogQuery("Bottom Half"))
                    .LogQuery("Shuffle")
                    .ToArray();
                */

                shuffle = shuffle.Skip(26)
                    .LogQuery("Bottom Half")
                    .InterleaveSequenceWith(shuffle.Take(26).LogQuery("Top Half"))
                    .LogQuery("Shuffle")
                    .ToArray();

                foreach (var c in shuffle)
                {
                    Console.WriteLine(c);
                }
                times++;
                Console.WriteLine(times);
            } while (!startingDeck.SequenceEquals(shuffle));

            Console.WriteLine(times);
            Console.ReadKey();

        }
    }
}
