using System;

namespace CleanCode.SwitchStatements
{
    public class Customer
    {
        public CustomerType Type { get; set; }

        public MonthlyStatement GenerateStatement(MonthlyUsage monthlyUsage)
        {
            var statement = new MonthlyStatement();

            switch (Type)
            {
                case CustomerType.PayAsYouGo:
                    statement.CallCost = 0.12f * monthlyUsage.CallMinutes;
                    statement.SmsCost = 0.12f * monthlyUsage.SmsCount;
                    statement.TotalCost = statement.CallCost + statement.SmsCost;
                    break;

                case CustomerType.Unlimited:
                    statement.TotalCost = 54.90f;
                    break;

                default:
                    throw new NotSupportedException("The current customer type is not supported");
            }

            return statement;
        }
    }

    public enum CustomerType
    {
        PayAsYouGo = 1,
        Unlimited
    }
    public class MonthlyUsage
    {
        public Customer Customer { get; set; }
        public int CallMinutes { get; set; }
        public int SmsCount { get; set; }
    }

    public class MonthlyStatement
    {
        public float CallCost { get; set; }
        public float SmsCost { get; set; }
        public float TotalCost { get; set; }
    }
}