using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        MainClass program = new MainClass();
		private bool dataSaved = true;

        public Form1()
        {
            InitializeComponent();

			saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				program.ReadFromFile(openFileDialog1.FileName, textBox1);
			}

			dataSaved = true;
		}

		private void button1_Click(object sender, EventArgs e)
        {
            program.circTest(textBox1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            program.coneTest(textBox1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
			program.clearInfo();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            program.circTask(textBox1, (int)numericUpDown1.Value, checkBox1.Checked);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            program.coneTask(textBox1, (int)numericUpDown2.Value, checkBox2.Checked);
        }

		private void Button6_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				program.ReadFromFile(openFileDialog1.FileName, textBox1);
			}
		}

		private void Button7_Click(object sender, EventArgs e)
		{
			saveData();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (dataSaved) return;

			saveData();
		}

		private void saveData()
		{
			if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
				return;

			string filename = saveFileDialog1.FileName;

			program.WriteToFile(filename, textBox1);

			dataSaved = true;
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			dataSaved = false;
		}

		//===================================================================

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{

		}

		private void numericUpDown2_ValueChanged(object sender, EventArgs e)
		{

		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
		{

		}
	}
}
