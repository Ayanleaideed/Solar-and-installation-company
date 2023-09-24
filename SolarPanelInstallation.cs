
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace SolarPanelInstallation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
           
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Charges.Visible = false;
        }

        private void CalculateCharges_Click(object sender, EventArgs e)
        {
            // Validate user inputs 
            if (!ValidateInputs()) return;

            // Get user inputs
            string firstName = firstNameText.Text;
            string lastName = lastNameText.Text;
            string phone = phoneText.Text;
            int numberOfPanels = int.Parse(numberPanelsText.Text);
            double depositAmount = double.Parse(depositAmoutIO.Text);
            bool expressInstallation = expressInstallatiobox.Checked;

            // Calculate charges
            double baseInstallationCharge = 2000;
            double additionalPanelCharge = (numberOfPanels - 2) * 300; // First 2 panels are included
            if (additionalPanelCharge < 0)
            {
                additionalPanelCharge = 0; // Ensure it's not negative
            }
            double totalInstallationCost = baseInstallationCharge + additionalPanelCharge;

            if (expressInstallation)
            {
                // Reduce waiting time with 5% express charge
                totalInstallationCost += totalInstallationCost * 0.05;
            }

            // Display charges in the group box
            Charges.Visible = true;
            baseChargeCalc.Text = baseInstallationCharge.ToString("C");
            additionlPnaelsCalc.Text = additionalPanelCharge.ToString("C");
            totalCost.Text = totalInstallationCost.ToString("C");
            depositAmout.Text = depositAmount.ToString("C");
            

            // Calculate balance due or refund
            double balanceDue = totalInstallationCost - depositAmount;
            if (balanceDue < 0)
            {
                balancedueCalc.Text = "Refund";
            }
            else
            {
                balancedueCalc.Text = balanceDue.ToString("C");

                // Save customer information and total cost to a text 
                string customerInfo = $"{firstName}, {lastName}, {phone}, {totalInstallationCost:C}";
                File.WriteAllText("customerInfo.txt", customerInfo);
                MessageBox.Show(customerInfo, "has been Saved");



            }
        }

        private bool ValidateInputs()
        {
            // Validate name and phone fields
            if (string.IsNullOrWhiteSpace(firstNameText.Text) || string.IsNullOrWhiteSpace(lastNameText.Text))
            {
                MessageBox.Show("Please enter both first name and last name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return false;
            }

            // Validate the number of panels
            if (!int.TryParse(numberPanelsText.Text, out int numberOfPanels) || numberOfPanels < 1 || numberOfPanels > 1000)
            {
                MessageBox.Show("Please enter a valid number of panels (1-1000).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
           
            // Validate the deposit amount
            if (!double.TryParse(depositAmoutIO.Text, out double depositAmount) || depositAmount <= 0)
            {
                MessageBox.Show("Please enter a valid deposit amount greater than 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Clear all inputs and hide the Charges group box
            firstNameText.Clear();
            lastNameText.Clear();
            phoneText.Clear();
            numberPanelsText.Clear();
            depositAmout.Clear();
            expressInstallatiobox.Checked = false;
            Charges.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }
    }
}
