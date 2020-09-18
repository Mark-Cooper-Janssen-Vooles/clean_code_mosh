using System;

namespace CleanCode.NestedConditionals
{
    public class Customer
    {
        public int LoyaltyPoints { get; set; }

        public bool IsGoldCustomer()
        {
            return LoyaltyPoints > 100;
        }
    }

    public class Reservation
    {
        public Reservation(Customer customer, DateTime dateTime)
        {
            From = dateTime;
            Customer = customer;
        }

        public DateTime From { get; set; }
        public Customer Customer { get; set; }
        public bool IsCanceled { get; set; }

        public void Cancel()
        {
            // Gold customers can cancel up to 24 hours before
            if (Customer.LoyaltyPoints > 100)
            {
                // If reservation already started throw exception
                if (DateTime.Now > From)
                {
                    throw new InvalidOperationException("It's too late to cancel.");
                }
                if ((From - DateTime.Now).TotalHours < 24)
                {
                    throw new InvalidOperationException("It's too late to cancel.");
                }
                IsCanceled = true;
            }
            else
            {
                // Regular customers can cancel up to 48 hours before

                // If reservation already started throw exception
                if (DateTime.Now > From)
                {
                    throw new InvalidOperationException("It's too late to cancel.");
                }
                if ((From - DateTime.Now).TotalHours < 48)
                {
                    throw new InvalidOperationException("It's too late to cancel.");
                }
                IsCanceled = true;
            }
        }

        public void CancelMark()
        {
            // Gold customers can cancel up to 24 hours before
            var isGoldCustomer = Customer.LoyaltyPoints > 100;
            var reservationAlreadyStarted = DateTime.Now > From;
            var reservationWithin24Hours = (From - DateTime.Now).TotalHours < 24;
            var reservationWithin48Hours = (From - DateTime.Now).TotalHours < 48;

            if (
                isGoldCustomer && (reservationAlreadyStarted || reservationWithin24Hours) ||
                reservationAlreadyStarted || (reservationWithin48Hours && !isGoldCustomer)
                )
                throw new InvalidOperationException("It's too late to cancel.");

            IsCanceled = true;
        }

        public void CancelMosh()
        {
            if (IsCancellationPeriodOver())
                throw new InvalidOperationException("It's too late to cancel.");

            IsCanceled = true;
        }

        private bool IsAlreadyStarted()
        {
            return DateTime.Now > From;
        }

        private bool IsCancellationPeriodOver()
        {
            return Customer.IsGoldCustomer() && LessThan(24) || !Customer.IsGoldCustomer() && LessThan(48) || IsAlreadyStarted();
        }

        private bool LessThan(int maxHours)
        {
            return (From - DateTime.Now).TotalHours < maxHours;
        }
    }
}
