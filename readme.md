# C# Developers - The Art of Writing Clean Code


"We write clean code for others more than ourselves"


Contents: 
- Common code smells
  - Poor names
  - Poor naming conventions
  - Poor method signatures
  - Long Parameter list
  - Output parameters
  - Variable declarations on the top
  - Magic numbers
  - Nested conditionals
  - Switch statements
  - Duplicated code
  - Comments
  - Long Methods
- Refactoring


### Poor Names
- Mysterious names
  - Names should be clear and intention-revealing
- Meaningless names
- Names with encodings
  - i.e. ``int iMaxRequests`` (the 'i')
- Ambiguous names
- Noisy names
  - i.e. ``Customer theCustomer;`` ... should just be ``Customer customer;``
- Summary:
  - Not too short, not too long
  - Meaningful
  - Reveal intention
  - Chosen from problem domain


### Poor naming conventions
- Other developers using naming conventions from other languages..
- C# conventions are PascalCase for method names, and camelCase for fields arguments/method parameters, and local variables. Private fields are prefixed with an underline 


### Poor Method Signatures
- ``Orange GetCustomer(int airplane);`` - airplane is an int, not an airplane. GetCustomer is an orange not a customer
- ``void Parse(int command);`` -> Parse usually takes an object and returns another one, here it is returning nothing as its void.
- Good Method signatures:
  - Check the return type
  - Check the method name
  - Check the parameters
  - Make sure these things are all logically related


### Long Parameter List
- If there is a class that has functions with parameters used by multiple classes you can break them out into a class. 
- "Extract class from parameters" using reSharper I.e.:
````c#
//from this:
public IEnumerable<Reservation> GetReservations(
  DateTime dateFrom, DateTime dateTo,
  User user, int locationId,
  LocationType locationType, int? customerId = null)
{
    if (dateFrom >= DateTime.Now)
        throw new ArgumentNullException("dateFrom");
    if (dateTo <= DateTime.Now)
        throw new ArgumentNullException("dateTo");

    throw new NotImplementedException();
}

//to this:
public class DateRange
{
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }

    public DateRange(DateTime dateFrom, DateTime dateTo)
    {
        DateFrom = dateFrom;
        DateTo = dateTo;
    }
}

public IEnumerable<Reservation> GetReservations( DateRange dateRange, User user, int locationId, LocationType locationType, int? customerId = null)
{
    if (dateRange.DateFrom >= DateTime.Now)
        throw new ArgumentNullException("dateFrom");
    if (dateRange.DateTo <= DateTime.Now)
        throw new ArgumentNullException("dateTo");

    throw new NotImplementedException();
}
````
- Just keep creating classes to have less parameters 
- Method Parameters Best Practices:
  - Less than 3 parameters


### Output Parameters
- Avoid at all costs
````c#
int count = 0;
var customers = GetCustomers(pageIndex, out count); //doesn't make sense, hard to read
````
- Can use a tuple instead: 
````c#
public class OutputParameters
{
    public void DisplayCustomers()
    {
        //int totalCount = 0;
        //var customers = GetCustomers(1, out totalCount); //out param = code smell!

        var tuple = GetCustomers2(1);
        var customers = tuple.Item1;
        var totalCount = tuple.Item2;

        Console.WriteLine("Total customers: " + totalCount);
        foreach (var c in customers)
            Console.WriteLine(c);
    }

    public IEnumerable<Customer> GetCustomers(int pageIndex, out int totalCount)
    {
        totalCount = 100;
        return new List<Customer>();
    }

    //a Tuple is a data structure with multiple fields:
    public Tuple<IEnumerable<Customer>, int> GetCustomers2(int pageIndex)
    {
        var totalCount = 100;
        return Tuple.Create((IEnumerable<Customer>) new List<Customer>(), totalCount);
    }
}
````
- However a tuple is considered a code smell too - what is "Item1" and "Item2"?
- In newer versions of c#, you can name tuples: 
````c#
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
````
- Or instead can also do: 
````c#
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
        //COULD ALSO DO, personal preference:
        // var result = GetCustomers2(pageIndex: 1);

        Console.WriteLine("Total customers: " + result.TotalCount);
        foreach (var c in result.Customers)
            Console.WriteLine(c);
    }

    public IEnumerable<Customer> GetCustomers(int pageIndex, out int totalCount)
    {
        totalCount = 100;
        return new List<Customer>();
    }

    public GetCustomersResult GetCustomers2(int pageIndex)
    {
        var totalCount = 100;

        return new GetCustomersResult()
        {
            Customers = new List<Customer>(),
            TotalCount = totalCount
        };
    }
}
````
- Take away on output parameters:
  - Avoid them!
  - Return an object from a method instead


### Variable Declarations on the top
- Doesn't make sense to declare things away from where they are used - doesn't read well, context switches etc.
- Declare variables close to their usage. Only if they are used in multiple places should they be near the top


### Magic Numbers
- An example of a magic number:
````c#
public void ApproveDocument(int status)
{
    if (status == 1) //a magic number, we have no idea what this represents as the reader.
    {
        // ...
    }
    else if (status == 2)
    {
        // ...
    }
}
````
- Fixing a magic number if only used locally: 
````c#
public void ApproveDocument(int status)
{
    const int draft = 1;
    const int lodged = 2;
    if (status == draft)
    {
        // ...
    }
    else if (status == lodged)
    {
        // ...
    }
}
````
- If its used elsewhere, a better approach is an enum
````c#
public enum DocumentStatus
{
    Draft = 1,
    Lodged = 2
}

public class MagicNumbers
{
    public void ApproveDocument(DocumentStatus status)
    {
        if (status == DocumentStatus.Draft)
        {
            // ...
        }
        else if (status == DocumentStatus.Lodged)
        {
            // ...
        }
    }
}
````
- Same deal but a "magic string":
````c#
public void RejectDoument(string status)
{
    switch (status)
    {
        case "1": //a magic string
            // ...
            break;
        case "2": //another magic string. What is "2"?
            // ...
            break;
    }
}
````
- better to use:
````c#
public enum DocumentStatus
{
    Draft = 1,
    Lodged = 2
}

public void RejectDoument(DocumentStatus status)
{
    switch (status)
    {
        case DocumentStatus.Draft: //a magic string
            // ...
            break;
        case DocumentStatus.Lodged:
            // ...
            break;
    }
}
````
- Avoid magic numbers:
    - Use constants or enums


### Nested Conditionals 
- They make programs hard to read, hard to understand, hard to change, and hard to test
- The more nested conditonals we have, the more execution paths we have, meaning the more tests we'll need
- Simplifying conditonal statements:
    - Use ternary operator when you can!
    ````c#
    if (a)
        c = someValue;
    else 
        c = anotherValue;

    //best to rewrite as ternary:
    c = a ? someValue : anotherValue;
    ````
    - AVOID ternary operator abuse, i.e. ``c = a ? b : d ? e : f;`` .. don't use more than one!
    - Simplify true / false:
    ````c#
    if(customer.TotalOrders > 50)
        isGoldCustomer = true;
    else
        isGoldCustomer = false;
    
    //better to use:
    isGoldCustomer = customer.TotalOrders > 50;
    ````
    - When combining statements?
    ````c#
    if (a)
    {
        if(b)
        {
            //statement
        }
    }

    //better to use "early exit"
    if(!a || !b)
        return;
    //statement - //if we're still running, that means both a and b are true
    ````
    - Swap orders technique
    ````c#
    if(a)
    {
        if(b)
        {
            isValid = true;
        }
    }
    if(c)
    {
        if(b)
        {
            isValid = true;
        }
    }

    //b always needs to be true, so can do this
    if(b && (a || c))
        isValid = true;

    //or even better:
    isTrue = b && (a || c);
    ````
    - everything in moderation though, the point is to make it readable...
    - i.e. this is not readable: ``isTrue = (a && (b || c) && !d || e && (f && !g || h))``
    - we write clean code for others more than ourselves
- An example refactoring he does:
    - when changing one thing, he reruns unit tests
````c#
public class Customer
{
    public int LoyaltyPoints { get; set; }
}

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
````
- Gets refactored into this:
````c#
public class Customer
{
    public int LoyaltyPoints { get; set; }

    public bool IsGoldCustomer()
    {
        return LoyaltyPoints > 100;
    }
}

public void Cancel()
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
````


### Switch Statements
- Popular among programmers familiar with OOP
- Why are switch statements not ideal?
    - if we want to add a new type, you need to change the switch method by adding a new case. Aka it requires recompiling and redeplying.
    - it doesn't follow the open-closed principle: open to extension, closed to modification
    - as we add more case statements, the switch statement will grow larger and larger and larger
    - when we have a switch statement, its possible this statement is spread all over the application 
- The solution to switch statements is polymorphism~ instead of having one customer class with a "type", we have a heirachy of customers, each customer type inheriting from the base customer class. i.e. a PayAsYouGoCustomer, UnlimitedCustomer
- When doing big refactorings, make sure you have tests! And run them regularly.
- Before refactor: 
````c#
public class Customer
{
    public CustomerType Type { get; set; }
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

    public void Generate(MonthlyUsage usage)
    {
        switch (usage.Customer.Type)
        {
            case CustomerType.PayAsYouGo:
                CallCost = 0.12f * usage.CallMinutes;
                SmsCost = 0.12f * usage.SmsCount;
                TotalCost = CallCost + SmsCost;
                break;

            case CustomerType.Unlimited:
                TotalCost = 54.90f;
                break;

            default:
                throw new NotSupportedException("The current customer type is not supported");
        }
    }
}
````
- after refactor:
````c#
public abstract class Customer
{
    public abstract MonthlyStatement GenerateStatement(MonthlyUsage monthlyUsage);
}

public class PayAsYouGoCustomer : Customer
{
    public override MonthlyStatement GenerateStatement(MonthlyUsage monthlyUsage)
    {
        var statement = new MonthlyStatement();
        statement.CallCost = 0.12f * monthlyUsage.CallMinutes;
        statement.SmsCost = 0.12f * monthlyUsage.SmsCount;
        statement.TotalCost = statement.CallCost + statement.SmsCost;

        return statement;
    }
}

public class UnlimitedCustomer : Customer
{
    public override MonthlyStatement GenerateStatement(MonthlyUsage monthlyUsage)
    {
        var statement = new MonthlyStatement();
        statement.TotalCost = 54.90f;

        return statement;
    }
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
````
- Replace switch statements with polymorphic dispatch (virtual and override methods)


### Duplicated Code
- Need to get rid of at all times
    - When you change it, you need to change it in multiple places (can forget areas too!)
    - Makes code noisy and hard to understand
- DRY: Don't repeat yourself
- If you refactor a method out of two functions in the same class, you might need to return more than one thing from this refactored object. This is prime time for a tuple or even better a new class!
    - Does the refactored method still belong in the original class? Maybe it makes more sense to have it in the new class


### Comments
- Unnecessary comments:
    - Comments that state the obvious - the code itself its self-explanatory (the goal!)
    - Version history comments 
    - comments to clarify the code 
    ````c#
    var pf = 10; //pay refrequency (instead just name the variable this!)
    ````
    - dead code (code commented out) - just delete it, it will be in github anyway
- Developers might not keep the comments up to date, and create confusion
- The ultimate comment for the code is the code itself (does not need comments!)
- TODO comments are okay... but they might never get done
    - can find these under view => task list (in visual studio) 
- Comments Best Practices
    - Don't write comments, re-write your code!
    - Don't explain "whats" (the obvious)
    - Explain "whys" and "hows"


### Long Methods
- Probably the most common code smell
- A method longer than 10 lines of code (Uncle Bob says 4 lines!)
- Problems:
    - hard to understand
    - hard to change
    - hard to reuse
- We want our methods to specialise in ONE thing
- When refactoring, think of the cohesion principle:
    - Things that are highly related should be together
    - Things that are not related, should not be together
    - Designing classes and methods in this way, we adhere to the "single responsibility principle":
        - A class / method should do only one thing, and do it very well.
- When telling a story through the code, do things in order so they make sense. i.e: 
````c#
protected void Page_Load(object sender, EventArgs e)
{
    var byteArray = GetCSV();
    ClearResponse();
    SetCacheability();
    WriteContentResponse(byteArray); //this is related to the byteArray, but isn't used until the end
}

//reads better:
protected void Page_Load(object sender, EventArgs e)
{
    ClearResponse();
    SetCacheability();
    WriteContentResponse(GetCSV());
}
````
- If a variable is spread all over the place and needs to be put into methods you can "inline variable" to get rid of it
- When you see a comment trying to explain a block of code, thats a great oppourtunity to extract the method
- Mosh likes symmetry:
````c#
private static void WriteRow(DataTable dt, StreamWriter sw, DataRow dr)
{
    for (int i = 0; i < dt.Columns.Count; i++)
    {
        WriteCell(sw, dr, i);
        WriteSeparatorIfRequired(dt, sw, i); //the order of arguments being called differs to WriteCell - this is inconsistent
    }
}
````
- Methods best practices:
    - Should be short (less than 5)
    - Should do only one thing
    - has meaningful name


### Refactor Examples
- Suggests we should have our classes methods ordered: 
    - The main method at the top, and the smaller methods it calls should be listed under the main method, in the order they are called.
    - so it reads "from top to bottom"