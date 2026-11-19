using System;
using System.Collections;
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
class BankAccount
{
    private static int nextNumber = 1000;

    private int accountNumber;
    private decimal balance;
    private AccountType accountType;
    private Queue transactions = new Queue();

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

        BankAccount acc1 = new BankAccount(1000m, AccountType.Current);
        BankAccount acc2 = new BankAccount(500m);
        BankAccount acc3 = new BankAccount(AccountType.Savings);
        BankAccount acc4 = new BankAccount();

        Console.WriteLine($"Номера счетов: {acc1.GetAccountNumber()}, {acc2.GetAccountNumber()}, {acc3.GetAccountNumber()}, {acc4.GetAccountNumber()}");
        Console.WriteLine($"Типы счетов: {acc1.GetAccountType()}, {acc2.GetAccountType()}, {acc3.GetAccountType()}, {acc4.GetAccountType()}");

        bool ok = acc2.Transfer(acc1, 300m);
        Console.WriteLine($"Перевод 300: {ok}");
        Console.WriteLine($"acc1: {acc1.GetBalance():F2}, acc2: {acc2.GetBalance():F2}");

        BankAccount nothing = null;
        Console.WriteLine($"Перевод с null: {acc2.Transfer(nothing, 100m)}");

        Console.WriteLine($"Перевод самому себе: {acc1.Transfer(acc1, 100m)}");

        BankAccount link = acc1;
        link.Deposit(100m);
        Console.WriteLine($"После link.Deposit(100): acc1 = {acc1.GetBalance():F2}");

        Money m1 = new Money();
        m1.Amount = 50m;
        Money m2 = m1;
        m2.Amount = 999m;
        Console.WriteLine($"m1.Amount = {m1.Amount}, m2.Amount = {m2.Amount}");
        Console.WriteLine();
        acc1.PrintTransactions();
        acc2.PrintTransactions();
    }
}