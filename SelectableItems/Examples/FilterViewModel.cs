using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Linq;
using DynamicData.Binding;

namespace DynamicData.Samplz.Examples
{
    public class FilterViewModel : AbstractNotifyPropertyChanged
    {
        private string _searchText;

        public FilterViewModel()
        {
            var  trades = new StockTicker("EUR",1.31, DateTime.Now);
        }


        public string SearchText
        {
            get { return _searchText; }
            set { SetAndRaise(ref _searchText, value); }
        }

        private IObservable<StockTicker> TradesGenerator()
        {
            //var generator =  0<Trade>()
            //bit of code to generate trades
            var random = new Random();

            return Observable.Empty<StockTicker>();

        } 
    } 

    public class StockTicker
    {
        public DateTime DateRecieved { get; }
        public double Price { get; }
        public string Symbol { get; }

        public StockTicker(string symbol, double price, DateTime dateRecieved)
        {
            Symbol = symbol;
            Price = price;
            DateRecieved = dateRecieved;
        }
    }

    public static class Ex
    {
        public static IEnumerable<int> Primes(int bound)
        {
            if (bound < 2) yield break;
            //The first prime number is 2
            yield return 2;

            BitArray composite = new BitArray((bound - 1) / 2);
            int limit = ((int)(Math.Sqrt(bound)) - 1) / 2;
            for (int i = 0; i < limit; i++)
            {
                if (composite[i]) continue;
                //The first number not crossed out is prime
                int prime = 2 * i + 3;
                yield return prime;
                //cross out all multiples of this prime, starting at the prime squared
                for (int j = (prime * prime - 2) >> 1; j < composite.Count; j += prime)
                {
                    composite[j] = true;
                }
            }
            //The remaining numbers not crossed out are also prime
            for (int i = limit; i < composite.Count; i++)
            {
                if (!composite[i]) yield return 2 * i + 3;
            }
        }
    }
}
