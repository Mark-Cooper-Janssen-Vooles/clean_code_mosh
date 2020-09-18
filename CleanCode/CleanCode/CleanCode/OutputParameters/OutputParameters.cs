using CleanCode.Comments;
using System;
using System.Collections.Generic;

namespace CleanCode.OutputParameters
{
    public class GetCustomersResult
    {
        public IEnumerable<Customer> Customers { get; set; }
        public int TotalCount { get; set; }
    }
    public class OutputParameters
    {
        public void DisplayCustomers()
        {
            const int pageIndex = 1;
            var result = GetCustomers2(pageIndex);

            //(var totalCount, var customers) = GetCustomers2(pageIndex);
            //result.

            //Console.WriteLine("Total customers: " + result.TotalCount);
            //foreach (var c in result.Customers)
            //    Console.WriteLine(c);
        }

        public IEnumerable<Customer> GetCustomers(int pageIndex, out int totalCount)
        {
            totalCount = 100;
            return new List<Customer>();
        }

        //public GetCustomersResult GetCustomers2(int pageIndex)
        //{
        //    var totalCount = 100;

        //    return new GetCustomersResult()
        //    {
        //        Customers = new List<Customer>(),
        //        TotalCount = totalCount
        //    };
        //}


        public Tuple<IEnumerable<Customer>,int> GetCustomers2(int pageIndex)
        {
            var totalCount = 100;

            return Tuple.Create((IEnumerable<Customer>)new List<Customer>(), totalCount);
        }
    }
}
