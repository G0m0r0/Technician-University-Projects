﻿namespace BankAccountManager
{
    using BankAccountManager.Core;
    using BankAccountManager.Core.Contracts;
    using System;
    using System.Security;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Linq;

    public partial class Wallet : Form
    {
        private AddAccount addAccountForm;
        private Transactions transactions;
        private readonly IEngine Engine;
        public Wallet(IEngine engin)
        {
            InitializeComponent();
            ///this.engine = new Engine();
            this.Engine = engin;
        }

        private void Wallet_Load(object sender, EventArgs e)
        {

        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();

            var accountsList = Engine.GetAllAccounts();

            foreach (var account in accountsList)
            {
                comboBox1.Items.Add(account);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do you want to close?", "Alert!", MessageBoxButtons.YesNo);

            if (dialog == DialogResult.Yes)
            {
                Environment.Exit(1);
            }
        }

        private void AddAccountButton_Click(object sender, EventArgs e)
        {
            this.addAccountForm = new AddAccount(Engine);
            addAccountForm.Show();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var iban = comboBox1.SelectedItem.ToString().Split("___", StringSplitOptions.RemoveEmptyEntries)[1].Remove(0,4);
            IbanLabel.Text = iban;

            DepositButton.Enabled = true;
            WithdrawButton.Enabled = true;
            AllMoneyButton.Enabled = true;

            BalanceTextBox.Text= comboBox1.SelectedItem.ToString().Split("___", StringSplitOptions.RemoveEmptyEntries)[2];
        }

        private void DepositButton_Click(object sender, EventArgs e)
        {
            var iban = IbanLabel.Text;

            string command = $"deposit {AmounthTextBox.Text} {iban}";

            Engine.Run(command);

            var account = Engine.GetAllAccounts().FirstOrDefault(x => DecriptSecureString(x.Iban) == iban);
            BalanceTextBox.Text = account.GetBalance.ToString()+'$';

            AmounthTextBox.Clear();
        }

        private void WithdrawButton_Click(object sender, EventArgs e)
        {
            var iban = IbanLabel.Text;

            string command = $"withdraw {AmounthTextBox.Text} {iban}";

            Engine.Run(command);

            var account = Engine.GetAllAccounts().FirstOrDefault(x => DecriptSecureString(x.Iban) == iban);
            BalanceTextBox.Text = account.GetBalance.ToString()+'$';

            AmounthTextBox.Clear();
        }
        private string DecriptSecureString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        private void AllMoneyButton_Click(object sender, EventArgs e)
        {
            var command = "balanceofallaccounts";
            Engine.Run(command);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.transactions = new Transactions(Engine);
            transactions.Show();
        }
    }
}
