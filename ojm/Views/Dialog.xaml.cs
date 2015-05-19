﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ojm.Views {
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog : Window {
        public Dialog(string question) {
            InitializeComponent();
            lblQuestion.Content = question;
            txtAnswer.Text = "";
        }
        private void btnDialogOk_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e) {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

        public string Answer {
            get { return txtAnswer.Text; }
        }
    }
}
