﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace AssetsClientApp
{
	public partial class MainForm : Form
	{
		// defining connection string based on settings
		private String connectionLine { get; set; }
		// defining list for query results here so we can use it within almost all the classes
		private List<dboTable> queryResultsList { get; set; }

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			
		}

		private void searchButton_Click(object sender, EventArgs e)
		{
			connectionLine = $"Server = '{ConfigurationManager.AppSettings["server"]}'; Database = '{ConfigurationManager.AppSettings["database"]}'; Trusted_Connection = Yes; Integrated Security = SSPI;";
			// getting list of scandates for searched hostname
			queryResultsList = dboTable.getFromSql($"SELECT * FROM dbo.main WHERE hostname = '{searchBox.Text}' ORDER BY id DESC;", connectionLine, "main");
			if (queryResultsList.Count != 0)
			{
				List<string> searchResulsList = new List<string>();
				foreach (dboTable tmp in queryResultsList)
				{
					searchResulsList.Add(Convert.ToString(tmp.scantime));
				}
				searchResultsBox.DataSource = searchResulsList;
				loadEntryButton.Enabled = true;
				searchResultsBox.Focus();
			}
			else
			{
				searchResultsBox.DataSource = new List<string> {"No entries found"};
				loadEntryButton.Enabled = false;
			}
		}

		private void searchResultsBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			loadEntryButton.Enabled = true;
		}

		// preparing data here to transfer to AssetForm
		private void loadEntryButton_Click(object sender, EventArgs e)
		{
			connectionLine = $"Server = '{ConfigurationManager.AppSettings["server"]}'; Database = '{ConfigurationManager.AppSettings["database"]}'; Trusted_Connection = Yes; Integrated Security = SSPI;";
			LoadingForm LoadSplashForm = new LoadingForm(queryResultsList[searchResultsBox.SelectedIndex].id, connectionLine);
			LoadSplashForm.ShowDialog();
			ViewAssetForm AssetForm = new ViewAssetForm(LoadSplashForm.sentData);
			AssetForm.Show();
		}

		// keys'n'double clicks hadlers below
		private void searchBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				searchButton_Click(this, new EventArgs());
			}
		}

		private void searchResultsBox_DoubleClick(object sender, EventArgs e)
		{
			if (loadEntryButton.Enabled)
			{
				loadEntryButton_Click(this, new EventArgs());
			}
		}

		private void searchResultsBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (loadEntryButton.Enabled)
				{
					loadEntryButton_Click(this, new EventArgs());
				}
			}
		}

		private void settingsButton_Click(object sender, EventArgs e)
		{
			SettingsForm Settings = new SettingsForm();
			Settings.ShowDialog();
		}

		// test shit below
	}
}
