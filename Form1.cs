using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace XPath
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button6;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.button6 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(312, 16);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(152, 24);
			this.button1.TabIndex = 0;
			this.button1.Text = "/catalog/cd/price";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// listBox1
			// 
			this.listBox1.Location = new System.Drawing.Point(304, 152);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(184, 95);
			this.listBox1.TabIndex = 1;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(312, 48);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(152, 24);
			this.button2.TabIndex = 2;
			this.button2.Text = "/catalog/cd[price>10.80]";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(400, 112);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(64, 24);
			this.button3.TabIndex = 3;
			this.button3.Text = "Exit";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(16, 40);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size = new System.Drawing.Size(264, 216);
			this.textBox1.TabIndex = 4;
			this.textBox1.Text = "textBox1";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "source xml data:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(304, 128);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 16);
			this.label2.TabIndex = 6;
			this.label2.Text = "result:";
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(312, 80);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(152, 24);
			this.button6.TabIndex = 9;
			this.button6.Text = "Edit Node";
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(504, 269);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			GetElements("/catalog/cd/price");
		}

		private void GetElements(string expression)
		{
			string fileName = "data.xml";
			XPathDocument doc = new XPathDocument(fileName);
			XPathNavigator nav = doc.CreateNavigator();
			
			// Compile a standard XPath expression
			XPathExpression expr; 
			expr = nav.Compile(expression);
			XPathNodeIterator iterator = nav.Select(expr);

			// Iterate on the node set
			listBox1.Items.Clear();
			try
			{
				while (iterator.MoveNext())
				{
					XPathNavigator nav2 = iterator.Current.Clone();

					listBox1.Items.Add("price: " + nav2.Value);
				
				}
			}
			catch(Exception ex) 
			{
				Console.WriteLine(ex.Message);
			} 
		}
		private void button2_Click(object sender, System.EventArgs e)
		{
			listBox1.Items.Clear();

			string fileName = "data.xml";
			XPathDocument doc = new XPathDocument(fileName);
			XPathNavigator nav = doc.CreateNavigator();
			
			// Compile a standard XPath expression
			XPathExpression expr; 
			expr = nav.Compile("/catalog/cd[price>10.80]");
			XPathNodeIterator iterator = nav.Select(expr);

			// Iterate on the node set
			listBox1.Items.Clear();
			try
			{
				while (iterator.MoveNext())
				{
					XPathNavigator nav2 = iterator.Current.Clone();

					nav2.MoveToFirstChild();
					listBox1.Items.Add("title: " + nav2.Value);
					nav2.MoveToNext();
					listBox1.Items.Add("artist: " + nav2.Value);
					nav2.MoveToNext();
					listBox1.Items.Add("price: " + nav2.Value);				
				}
			}
			catch(Exception ex) 
			{
				Console.WriteLine(ex.Message);
			} 
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		const string FILE_NAME = "data.xml";
		private void Form1_Load(object sender, System.EventArgs e)
		{
			
			LoadXml(FILE_NAME);
		}

		private void LoadXml(string FILE_NAME)
		{
			if (!File.Exists(FILE_NAME)) 
			{
				Console.WriteLine("{0} does not exist.", FILE_NAME);
				return;
			}
			StreamReader sr = File.OpenText(FILE_NAME);
			String input;
			
			input = sr.ReadToEnd();
			sr.Close();
			textBox1.Text = input;
		}
		private void button6_Click(object sender, System.EventArgs e)
		{
			EditForm edit = new EditForm();
			DialogResult result = edit.ShowDialog();
			if(result==DialogResult.OK)
			{
				LoadXml(FILE_NAME);
			}
		}
	}
}
