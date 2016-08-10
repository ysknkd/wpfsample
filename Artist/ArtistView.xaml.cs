﻿using Common;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Artist
{
    /// <summary>
    /// ArtistView.xaml の相互作用ロジック
    /// </summary>
    public partial class ArtistView : UserControl
    {
        public ArtistView()
        {
            InitializeComponent();

            var viewModel = (ViewModelBase)DataContext;
            viewModel.FindResources = FindResource;
        }
    }
}
