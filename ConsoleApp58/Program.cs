namespace ConsoleApp58
{
    using System;
    using System.Collections.Generic;

    namespace BankAccountApp
    {
      
        public class Account
        {
            public string Name { get; set; }
            public decimal Balance { get; set; }

            public Account(string name, decimal balance)
            {
                Name = name;
                Balance = balance;
            }

            public virtual bool Deposit(decimal amount)
            {
                if (amount <= 0) return false;
                Balance += amount;
                return true;
            }

            public virtual bool Withdraw(decimal amount)
            {
                if (amount <= 0 || amount > Balance) return false;
                Balance -= amount;
                return true;
            }

            public override string ToString()
            {
                return $"{GetType().Name} - {Name}: ${Balance}";
            }
        }

       
        public class SavingsAccount : Account
        {
            public decimal InterestRate { get; set; }

            public SavingsAccount(string name, decimal balance, decimal interestRate)
                : base(name, balance)
            {
                InterestRate = interestRate;
            }

            public override bool Deposit(decimal amount)
            {
                if (base.Deposit(amount))
                {
                    Balance += amount * InterestRate / 100;
                    return true;
                }
                return false;
            }
        }

        public class CheckingAccount : Account
        {
            private const decimal WithdrawalFee = 1.50m;

            public CheckingAccount(string name, decimal balance)
                : base(name, balance) { }

            public override bool Withdraw(decimal amount)
            {
                return base.Withdraw(amount + WithdrawalFee);
            }
        }

      
        public class TrustAccount : SavingsAccount
        {
            private int withdrawalCount = 0;
            private const int MaxWithdrawals = 3;

            public TrustAccount(string name, decimal balance, decimal interestRate)
                : base(name, balance, interestRate) { }

            public override bool Deposit(decimal amount)
            {
                if (amount >= 5000)
                    Balance += 50; 
                return base.Deposit(amount);
            }

            public override bool Withdraw(decimal amount)
            {
                if (withdrawalCount >= MaxWithdrawals) return false;
                if (amount > Balance * 0.2m) return false;
                if (base.Withdraw(amount))
                {
                    withdrawalCount++;
                    return true;
                }
                return false;
            }
        }

        
        class Program
        {
            static void Main(string[] args)
            {
                List<Account> accounts = new List<Account> {
                new SavingsAccount("Ahmed", 1000, 5),
                new CheckingAccount("Sara", 2000),
                new TrustAccount("Khaled", 10000, 3)
            };

                foreach (var acc in accounts)
                {
                    Console.WriteLine(acc);
                    acc.Deposit(1000);
                    acc.Withdraw(500);
                    Console.WriteLine("After transactions: " + acc);
                    Console.WriteLine();
                }

                
                var trust = new TrustAccount("Test Trust", 10000, 4);
                trust.Withdraw(1500); 
                trust.Withdraw(1500); 
                trust.Withdraw(1500); 
                bool failed = trust.Withdraw(1500); 
                Console.WriteLine("Extra withdrawal allowed? " + failed);
            }
        }
    }
}
