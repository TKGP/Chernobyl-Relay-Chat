using System.Collections.Generic;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    public partial class KeyPromptForm : Form
    {
        public string Result;

        public KeyPromptForm()
        {
            InitializeComponent();
            label1.Hide();
        }

        private void KeyPromptForm_KeyDown(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;
            if(keyCode >= Keys.A && keyCode <= Keys.Z
                || keyCode >= Keys.F1 && keyCode <= Keys.F15)
            {
                Result = keyCode.ToString();
                Close();
            }
            else if (keyToDik.ContainsKey(keyCode))
            {
                Result = keyToDik[keyCode];
                Close();
            }
            else
                label1.Show();
        }

        private static Dictionary<Keys, string> keyToDik = new Dictionary<Keys, string>()
        {
            [Keys.Oem3] = "GRAVE",
            [Keys.D0] = "0",
            [Keys.D1] = "1",
            [Keys.D2] = "2",
            [Keys.D3] = "3",
            [Keys.D4] = "4",
            [Keys.D5] = "5",
            [Keys.D6] = "6",
            [Keys.D7] = "7",
            [Keys.D8] = "8",
            [Keys.D9] = "9",
            [Keys.OemMinus] = "MINUS",
            [Keys.Oemplus] = "EQUALS",

            [Keys.NumPad0] = "NUMPAD0",
            [Keys.NumPad1] = "NUMPAD1",
            [Keys.NumPad2] = "NUMPAD2",
            [Keys.NumPad3] = "NUMPAD3",
            [Keys.NumPad4] = "NUMPAD4",
            [Keys.NumPad5] = "NUMPAD5",
            [Keys.NumPad6] = "NUMPAD6",
            [Keys.NumPad7] = "NUMPAD7",
            [Keys.NumPad8] = "NUMPAD8",
            [Keys.NumPad9] = "NUMPAD9",
            [Keys.Decimal] = "DECIMAL",
            [Keys.Add] = "ADD",
            [Keys.Subtract] = "SUBTRACT",
            [Keys.Multiply] = "MULTIPLY",
            [Keys.Divide] = "DIVIDE",

            // Both main enter and numpad enter are Keys.Enter, at least on my keyboard, so idk what Keys.Return is
            [Keys.Enter] = "RETURN",
            [Keys.Escape] = "ESCAPE",
            [Keys.Tab] = "TAB",
            [Keys.Space] = "SPACE",
            [Keys.Oem2] = "SLASH",
            [Keys.Oem7] = "APOSTROPHE",
            [Keys.Back] = "BACKSPACE",
            [Keys.Oem5] = "BACKSLASH",
            [Keys.Oem4] = "LBRACKET",
            [Keys.Oem6] = "RBRACKET",
            [Keys.Oem1] = "SEMICOLON",
            [Keys.Oemcomma] = "COMMA",
            [Keys.OemPeriod] = "PERIOD",

            [Keys.Delete] = "DELETE",
            [Keys.End] = "END",
            [Keys.Home] = "HOME",
            [Keys.Insert] = "INSERT",
            [Keys.Left] = "LEFT",
            [Keys.Right] = "RIGHT",
            [Keys.Up] = "UP",
            [Keys.Down] = "DOWN",
        };
    }
}
