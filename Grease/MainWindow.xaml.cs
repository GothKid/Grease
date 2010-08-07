﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using Grease.Utils;


namespace Grease
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MusicLibrary library;
        private Mp3Info currSong;
        private UserSettingsGroup settings;
        public MainWindow()
        {
            InitializeComponent();
            library = new MusicLibrary();

            var md = Settings.Default.MusicDirectory;
            if (!string.IsNullOrEmpty(md) && md != "None")
            {
                LoadSongs(md);
            }
        }

        private void LoadSongs(string folder)
        {
            library.Songs = FolderHelper.GetSongs(folder);
            lblSongCount.Content = "Found " + library.Songs.Count.ToString() + " mp3's";
        }

        private void btnChooseDirectory_Click(object sender, RoutedEventArgs e)
        {
            var pathFinder = new FolderBrowserDialog();
            pathFinder.RootFolder = Environment.SpecialFolder.MyDocuments;
            var path = pathFinder.ShowDialog();
            if (!string.IsNullOrEmpty(pathFinder.SelectedPath))
            {
                LoadSongs(pathFinder.SelectedPath);
                Settings.Default.MusicDirectory = pathFinder.SelectedPath;
                Settings.Default.Save();
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (library.Songs != null && library.Songs.Count > 0)
            {
                if (currSong == null)
                {
                    Player.Source = new Uri(library.GetRandomMp3().FullPath);
                    currSong = library.GetRandomMp3();
                    lblCurrentlyPlaying.Content = currSong.Name;
                }
                Player.Play();
            }
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            Player.Pause();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Player.Pause();
            if (library.Songs.Count > 0)
            {
                currSong = library.GetRandomMp3();
                Player.Source = new Uri(currSong.FullPath);
                lblCurrentlyPlaying.Content = currSong.Name;
                Player.Play();
            }
        }

        private void CompletedSong(object sender, RoutedEventArgs e)
        {
            btnNext_Click(sender, e);
        }
    }
}
