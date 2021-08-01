using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegisterFormWF
{
    public partial class Form1 : Form
    {
        User user = new User();
        public Form1()
        {
            InitializeComponent();
        }
        
        string oldText = string.Empty;
        private string OnlyLetterGuna2TextBox(Guna2TextBox textBox)
        {
            if (textBox.Text.All(chr => char.IsLetter(chr)))
            {
                oldText = textBox.Text;
                textBox.Text = oldText;
            }
            else
            {
                textBox.Text = oldText;
            }
            textBox.SelectionStart = textBox.Text.Length;
            return textBox.Text;
        }
        private void GunaCheckBoxConfirmation_CheckedChanged(object sender, EventArgs e)
        {
            if (gunaCheckBoxConfirmation.Checked)
            {
                gunaCircleButtonSave.Enabled = true;
                gunaCircleButtonLoad.Enabled = true;
            }
            else
            {
                gunaCircleButtonSave.Enabled = false;
                gunaCircleButtonLoad.Enabled = false;
            }
        }
        private void GunaTextBoxName_TextChanged(object sender, EventArgs e)
        {
            user.Name = OnlyLetterGuna2TextBox(gunaTextBoxName);
        }
        private void GunaTextBoxSurname_TextChanged(object sender, EventArgs e)
        {
            user.Surname = OnlyLetterGuna2TextBox(gunaTextBoxSurname);
        }
        private void GunaDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            user.BirthDate = gunaDateTimePicker.Value;
        }
        private void GunaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox cb)
            {
                if (cb.Checked == true)
                {
                    user.FavMusics.Add(cb.Text);
                }
            }
        }
        private void GunaCustomRadioButtonMale_CheckedChanged(object sender, EventArgs e)
        {
            if (gunaCustomRadioButtonMale.Checked == true)
            {
                user.Gender = gunaTextBoxMale.Text;
            }
        }
        private void GunaCustomRadioButtonFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (gunaCustomRadioButtonFemale.Checked == true)
            {
                user.Gender = gunaTextBoxFemale.Text;
            }
        }
        private void GunaTextBoxAdress_TextChanged(object sender, EventArgs e)
        {
            user.Adress = OnlyLetterGuna2TextBox(gunaTextBoxAdress);
        }
        private void GunaTextBoxCountry_TextChanged(object sender, EventArgs e)
        {
            user.Country = OnlyLetterGuna2TextBox(gunaTextBoxCountry);
        }
        private void GunaTextBoxCity_TextChanged(object sender, EventArgs e)
        {
            user.City = OnlyLetterGuna2TextBox(gunaTextBoxCity);
        }
        private void GunaTextBoxEmail_TextChanged(object sender, EventArgs e)
        {
            user.Email = gunaTextBoxEmail.Text;
        }

        private void GunaCircleButtonSave_Click(object sender, EventArgs e)
        {
            if (gunaGroupBoxUserPersonal.Controls.OfType<Guna2TextBox>().Any(gtb1 => string.IsNullOrEmpty(gtb1.Text)
            || gunaGroupBoxUserContact.Controls.OfType<Guna2TextBox>().Any(gtb2 => string.IsNullOrEmpty(gtb2.Text)
            || gunaGroupBoxUserContact.Controls.OfType<MaskedTextBox>().Any(mtb => !mtb.MaskCompleted)
            || gunaDateTimePicker.Value >= DateTime.Now
            || !groupBoxGender.Controls.OfType<Guna2CustomRadioButton>().Any(rb => rb.Checked == true)
            || !(groupBoxFavMusicTypes.Controls.OfType<Guna2CheckBox>().Any(gcb => gcb.Checked == true)
            ))))
            {
                var dialog = MessageBox.Show("Fill in the information completely!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                var textJson = JsonSerializer.Serialize(user, options);
                File.WriteAllText(user.Name + ".json", textJson);
                MessageBox.Show("Registration ended successfully!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MaskedTextBoxPhone_TextChanged(object sender, EventArgs e)
        {
            user.Phone = maskedTextBoxPhone.Text;
        }

        private void MaskedTextBoxPostalCode_TextChanged(object sender, EventArgs e)
        {
            user.PostalCode = maskedTextBoxPostalCode.Text;
        }

        private void GunaCircleButtonClear_Click(object sender, EventArgs e)
        {
            gunaGroupBoxUserPersonal.Controls.OfType<Guna2TextBox>().ToList().ForEach(gtb => gtb.Text = string.Empty);
            gunaGroupBoxUserContact.Controls.OfType<Guna2TextBox>().ToList().ForEach(gtb => gtb.Text = string.Empty);
            gunaGroupBoxUserContact.Controls.OfType<MaskedTextBox>().ToList().ForEach(gmtb => gmtb.Text = string.Empty);
            gunaDateTimePicker.Value = DateTime.Now;
            gunaCustomRadioButtonMale.Checked = false;
            gunaCustomRadioButtonFemale.Checked = false;
            groupBoxFavMusicTypes.Controls.OfType<Guna2CheckBox>().ToList().ForEach(gcb => gcb.Checked = false);

        }

        private void GunaCircleButtonLoad_Click(object sender, EventArgs e)
        {
            if (gunaTextBoxName.Text == string.Empty)
            {
                MessageBox.Show("Please include name!", "Info", MessageBoxButtons.OK);
            }
            else
            {
                if (File.Exists(gunaTextBoxName.Text + ".json"))
                {
                    User user = new User();
                    var text = File.ReadAllText(gunaTextBoxName.Text + ".json");
                    user = JsonSerializer.Deserialize<User>(text);
                    gunaTextBoxName.Text = user.Name;
                    gunaTextBoxSurname.Text = user.Surname;
                    gunaDateTimePicker.Value = user.BirthDate;
                    maskedTextBoxPhone.Text = user.Phone;
                    gunaTextBoxAdress.Text = user.Adress;
                    gunaTextBoxCity.Text = user.City;
                    gunaTextBoxEmail.Text = user.Email;
                    gunaTextBoxCountry.Text = user.Country;
                    maskedTextBoxPostalCode.Text = user.PostalCode;
                    if (user.Gender == "Male") gunaCustomRadioButtonMale.Checked = true;
                    else gunaCustomRadioButtonFemale.Checked = true;
                    foreach (var item in user.FavMusics)
                    {
                        groupBoxFavMusicTypes.Controls.OfType<Guna2CheckBox>().ToList().Find(gcb2 => gcb2.Text == item).Checked = true;
                    }
                }
                else
                {
                    MessageBox.Show("User not found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
