using System;
using System.Collections;
using System.IO;
enum AccountType
{
    Current,
    Savings,
    Credit,
    Deposit
}

class BankTransaction
{
    public readonly DateTime Date;
    public readonly decimal Amount;

    public BankTransaction(decimal amount)
    {
        Date = DateTime.Now;
        Amount = amount;
    }
}
class BankAccount : IDisposable
{
    private static int nextNumber = 1000;

    private int accountNumber;
    private decimal balance;
    private AccountType accountType;
    private Queue transactions = new Queue();
    private bool disposed = false;

    private static int GenerateNumber()
    {
        return ++nextNumber;
    }

    public BankAccount()
    {
        accountNumber = GenerateNumber();
        balance = 0;
        accountType = AccountType.Current;
    }

    public BankAccount(decimal balance)
    {
        accountNumber = GenerateNumber();
        this.balance = balance;
        accountType = AccountType.Current;
    }

    public BankAccount(AccountType accountType)
    {
        accountNumber = GenerateNumber();
        balance = 0;
        this.accountType = accountType;
    }

    public BankAccount(decimal balance, AccountType accountType)
    {
        accountNumber = GenerateNumber();
        this.balance = balance;
        this.accountType = accountType;
    }

    public int GetAccountNumber()
    {
        return accountNumber;
    }

    public decimal GetBalance()
    {
        return balance;
    }

    public AccountType GetAccountType()
    {
        return accountType;
    }

    public bool Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            return false;
        }
        balance += amount;
        transactions.Enqueue(new BankTransaction(amount));
        return true;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0 || amount > balance)
        {
            return false;
        }
        balance -= amount;
        transactions.Enqueue(new BankTransaction(-amount));
        return true;
    }

    public bool Transfer(BankAccount source, decimal amount)
    {
        if (source == null)
        {
            return false;
        }

        if (source == this)
        {
            return false;
        }

        if (!source.Withdraw(amount))
        {
            return false;
        }

        Deposit(amount);
        return true;
    }
    public void PrintTransactions()
    {
        Console.WriteLine($"История операций счета {accountNumber}:");

        if (transactions.Count == 0)
        {
            Console.WriteLine("  операций нет");
            return;
        }

        foreach (object item in transactions)
        {
            BankTransaction t = (BankTransaction)item;
            Console.WriteLine($"  {t.Date:dd.MM.yyyy HH:mm:ss}   {t.Amount:+0.00;-0.00}");
        }
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        string fileName = $"transactions_{accountNumber}.txt";

        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (object item in transactions)
            {
                BankTransaction t = (BankTransaction)item;
                writer.WriteLine($"{t.Date:dd.MM.yyyy HH:mm:ss};{t.Amount:+0.00;-0.00}");
            }
        }

        Console.WriteLine($"Проводки счета {accountNumber} записаны в файл: {Path.GetFullPath(fileName)}");

        disposed = true;
        GC.SuppressFinalize(this);
    }
}

struct Money
{
    public decimal Amount;
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        using (BankAccount acc1 = new BankAccount(1000m, AccountType.Current))
        using (BankAccount acc2 = new BankAccount(500m, AccountType.Savings))
        {
            acc1.Deposit(200m);
            acc1.Withdraw(150m);
            acc1.Withdraw(5000m);
            acc2.Transfer(acc1, 300m);

            acc1.PrintTransactions();
            Console.WriteLine();
            acc2.PrintTransactions();
            Console.WriteLine();
        }

        Console.WriteLine("Блок using закончился, Dispose вызван автоматически.");
    }
}