using System;
using System.Collections.Generic;

namespace BankAccountManagementSystem
{
    // BankAccount class as an abstraction of a bank account
    public class BankAccount
    {
        private string accountNumber;
        private string accountHolderName;
        private decimal balance;
        private List<Transaction> transactions;

        public string AccountNumber
        {
            get { return accountNumber; }
            private set { accountNumber = value; }
        }

        public string AccountHolderName
        {
            get { return accountHolderName; }
            set { accountHolderName = value; }
        }

        public decimal Balance
        {
            get { return balance; }
            private set { balance = value; }
        }

        public BankAccount(string accountNumber, string accountHolderName, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            AccountHolderName = accountHolderName;
            Balance = initialBalance;
            transactions = new List<Transaction>();
        }

        // Deposit method overloads
        public void Deposit(decimal amount)
        {
            Deposit(amount, "Deposit");
        }

        public void Deposit(decimal amount, string description)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Deposit amount must be greater than zero.");
                return;
            }

            Balance += amount;
            AddTransaction(amount, description);
        }

        // Withdraw method overloads
        public void Withdraw(decimal amount)
        {
            Withdraw(amount, "Withdrawal");
        }

        public virtual void Withdraw(decimal amount, string description)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Withdrawal amount must be greater than zero.");
                return;
            }

            if (amount > Balance)
            {
                Console.WriteLine("Insufficient funds.");
                return;
            }

            Balance -= amount;
            AddTransaction(-amount, description);
        }

        protected void AddTransaction(decimal amount, string description)
        {
            Transaction transaction = new Transaction(amount, description);
            transactions.Add(transaction);
        }

        public virtual void CalculateInterest()
        {
            // Default interest calculation for the base BankAccount class
            Console.WriteLine("Interest calculated based on the base BankAccount class.");
        }

        public void PrintStatement()
        {
            Console.WriteLine($"Account Number: {AccountNumber}");
            Console.WriteLine($"Account Holder: {AccountHolderName}");
            Console.WriteLine($"Balance: {Balance:C}");
            Console.WriteLine("Transaction History:");

            foreach (Transaction transaction in transactions)
            {
                Console.WriteLine(transaction);
            }
        }
    }

    // Subclass SavingsAccount inheriting from BankAccount
    public class SavingsAccount : BankAccount, ITransaction
    {
        public SavingsAccount(string accountNumber, string accountHolderName, decimal initialBalance)
            : base(accountNumber, accountHolderName, initialBalance)
        {
        }

        public override void CalculateInterest()
        {
            decimal interestRate = 0.05m;
            decimal interest = Balance * interestRate;
            Deposit(interest, "Interest Deposit");
        }

        public void ExecuteTransaction(decimal amount)
        {
            if (amount >= 0)
            {
                Deposit(amount, "Transaction");
            }
            else
            {
                Withdraw(-amount, "Transaction");
            }
        }

        public void PrintTransaction()
        {
            Console.WriteLine("Transaction Details: Savings Account");
        }
    }

    // Subclass CheckingAccount inheriting from BankAccount
    public class CheckingAccount : BankAccount, ITransaction
    {
        public CheckingAccount(string accountNumber, string accountHolderName, decimal initialBalance)
            : base(accountNumber, accountHolderName, initialBalance)
        {
        }

        public override void CalculateInterest()
        {
            Console.WriteLine("No interest calculated for Checking Account.");
        }

        public void ExecuteTransaction(decimal amount)
        {
            if (amount >= 0)
            {
                Deposit(amount, "Transaction");
            }
            else
            {
                Withdraw(-amount, "Transaction");
            }
        }

        public void PrintTransaction()
        {
            Console.WriteLine("Transaction Details: Checking Account");
        }
    }

    // Subclass LoanAccount inheriting from BankAccount
    public class LoanAccount : BankAccount, ITransaction
    {
        public LoanAccount(string accountNumber, string accountHolderName, decimal initialBalance)
            : base(accountNumber, accountHolderName, initialBalance)
        {
        }

        public override void CalculateInterest()
        {
            decimal interestRate = 0.1m;
            decimal interest = Balance * interestRate;
            Deposit(interest, "Interest Deposit");
        }

        public void ExecuteTransaction(decimal amount)
        {
            if (amount >= 0)
            {
                Deposit(amount, "Transaction");
            }
            else
            {
                Withdraw(-amount, "Transaction");
            }
        }

        public void PrintTransaction()
        {
            Console.WriteLine("Transaction Details: Loan Account");
        }
    }

    // Transaction class to store transaction details
    public class Transaction
    {
        public decimal Amount { get; }
        public string Description { get; }

        public Transaction(decimal amount, string description)
        {
            Amount = amount;
            Description = description;
        }

        public override string ToString()
        {
            return $"{Description}: {Amount:C}";
        }
    }

    // ITransaction interface
    public interface ITransaction
    {
        void ExecuteTransaction(decimal amount);
        void PrintTransaction();
    }

    // Bank class managing different bank accounts
    public class Bank
    {
        private Dictionary<string, BankAccount> bankAccounts;

        public Bank()
        {
            bankAccounts = new Dictionary<string, BankAccount>();
        }

        public void AddBankAccount(BankAccount account)
        {
            bankAccounts.Add(account.AccountNumber, account);
        }

        public BankAccount GetBankAccount(string accountNumber)
        {
            if (bankAccounts.ContainsKey(accountNumber))
            {
                return bankAccounts[accountNumber];
            }

            return null;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Bank bank = new Bank();

            // Creating instances of different account types
            SavingsAccount savingsAccount = new SavingsAccount("SA001", "John Doe", 5000m);
            CheckingAccount checkingAccount = new CheckingAccount("CA001", "Jane Smith", 2000m);
            LoanAccount loanAccount = new LoanAccount("LA001", "Alice Johnson", 10000m);

            // Adding the bank accounts to the bank
            bank.AddBankAccount(savingsAccount);
            bank.AddBankAccount(checkingAccount);
            bank.AddBankAccount(loanAccount);

            // Performing transactions and printing statements
            savingsAccount.Deposit(1000m);
            savingsAccount.Withdraw(500m);
            savingsAccount.CalculateInterest();
            savingsAccount.PrintStatement();

            Console.WriteLine();

            checkingAccount.Deposit(200m);
            checkingAccount.Withdraw(300m);
            checkingAccount.PrintStatement();

            Console.WriteLine();

            loanAccount.Withdraw(500m);
            loanAccount.CalculateInterest();
            loanAccount.PrintStatement();

            Console.WriteLine();

            // Testing polymorphism and interface implementation
            ITransaction transaction1 = new SavingsAccount("SA002", "Mark Davis", 5000m);
            transaction1.ExecuteTransaction(1000m);
            transaction1.PrintTransaction();

            Console.WriteLine();

            ITransaction transaction2 = new CheckingAccount("CA002", "Amy Johnson", 2000m);
            transaction2.ExecuteTransaction(-500m);
            transaction2.PrintTransaction();

            Console.WriteLine();

            ITransaction transaction3 = new LoanAccount("LA002", "Michael Smith", 10000m);
            transaction3.ExecuteTransaction(-1000m);
            transaction3.PrintTransaction();
        }
    }
}
