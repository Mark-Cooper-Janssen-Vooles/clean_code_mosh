using System;
using System.Collections.Generic;

namespace Tuples
{
    public class Customer
    {

    }
    public class OutputParameters
    {
        public void DisplayCustomers()
        {
            const int pageIndex = 1;
            var result = GetCustomers2(pageIndex);

            (var customers, var totalCount) = GetCustomers2(pageIndex); // syntax for named tuples

            Console.WriteLine("Total customers: " + totalCount);
            foreach (var c in customers)
                Console.WriteLine(c);
        }


        public Tuple<IEnumerable<Customer>, int> GetCustomers2(int pageIndex)
        {
            var totalCount = 100;

            return Tuple.Create((IEnumerable<Customer>)new List<Customer>(), totalCount);
        }
    }
        class Program
    {
        static void Main(string[] args)
        {
               
        }
    }
}
