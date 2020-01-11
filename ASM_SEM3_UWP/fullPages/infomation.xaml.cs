﻿using ASM_SEM3_UWP.model;
using ASM_SEM3_UWP.service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ASM_SEM3_UWP.fullPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class infomation : Page
    {
        private static readonly string uriGetSong = "https://2-dot-backup-server-002.appspot.com/_api/v2/songs/get-mine";
        playMusic playMusicSite = new playMusic();
        infomationService infomationservice = new infomationService();
        private song currentSong;
        public static bool _isPlaying = false;
        private ObservableCollection<song> currentListSong=null;
        public infomation()
        {
            this.InitializeComponent();
            run();
        }
        private async void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
           currentListSong= await infomationservice.GetListSong(Login.token, uriGetSong);
            Songs.ItemsSource = currentListSong;
            progress1.IsActive = false;

        }
        
        private async System.Threading.Tasks.Task run()
        {
            progress1.IsActive = true;

            user member = await this.infomationservice.GetMemberInformation(Login.token);
            if (member != null)
            {
            memberName.Text = member.firstName +" "+ member.lastName;
            memberAddress.Text = member.address;
            memberPhone.Text = member.phone;
            memberEmail.Text = member.email;
                avatar.Source = new BitmapImage(new Uri(member.avatar, UriKind.Absolute));
            }
           
        }
       
        public void stopmusic()
        {

            if (Songs.ItemsSource == null) return;
            if (currentSong == null)
            {
                currentSong = Songs.Items[0] as song;
                MyPlayer.Source = MediaSource.CreateFromUri(new Uri(currentSong.link));
                Songs.SelectedIndex = 0;
            }
            if (_isPlaying)
            {
                MyPlayer.MediaPlayer.Pause();
                PlayButton.Icon = new SymbolIcon(Symbol.Play);
                StatusText.Text = "Paused";
                _isPlaying = false;
            }
        }
        private void Songs_OnItemClick(object sender, ItemClickEventArgs e)
        {
            List<song> listSongPlay = new List<song>(currentListSong);
            
            currentSong = e.ClickedItem as song;
            Debug.WriteLine("currentSong.id=" + currentSong.id);
            playMusicSite.listend(listSongPlay,currentSong.id);

            //MyPlayer.Source = MediaSource.CreateFromUri(new Uri(currentSong.link));
            //MyPlayer.MediaPlayer.Play();
            //PlayButton.Icon = new SymbolIcon(Symbol.Pause);
            //_isPlaying = true;
            //StatusText.Text = "Now Playing: " + currentSong.name;
        }
        private void PlayButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (Songs.ItemsSource == null) return;
            if (currentSong == null)
            {
                currentSong = Songs.Items[0] as song;
                MyPlayer.Source = MediaSource.CreateFromUri(new Uri(currentSong.link));
                Songs.SelectedIndex = 0;
            }
            if (_isPlaying)
            {
                MyPlayer.MediaPlayer.Pause();
                PlayButton.Icon = new SymbolIcon(Symbol.Play);
                StatusText.Text = "Paused";
                _isPlaying = false;
            }
            else
            {
                MyPlayer.MediaPlayer.Play();
                PlayButton.Icon = new SymbolIcon(Symbol.Pause);
                StatusText.Text = "Now Playing: " + currentSong.name;
                _isPlaying = true;
            }
        }
        private void Next_OnClick(object sender, RoutedEventArgs e)
        {
            var currentIndex = Songs.SelectedIndex;
            currentIndex++;
            if (currentIndex >= Songs.Items.Count)
            {
                currentIndex = 0;
            }
            playCurrentMusic(currentIndex);
            Debug.WriteLine("link=" +currentSong.link);
        }
       
        private void Previous_OnClick(object sender, RoutedEventArgs e)
        {
            var currentIndex = Songs.SelectedIndex;
            currentIndex--;
            if (currentIndex <=0)
            {
                currentIndex = 5;
            }
            playCurrentMusic(currentIndex);
            Debug.WriteLine("link=" + currentSong.link);

        }
        private void playCurrentMusic(int index)
        {
            currentSong = Songs.Items[index] as song;
            Songs.SelectedIndex = index;
            MyPlayer.Source = MediaSource.CreateFromUri(new Uri(currentSong.link));
            MyPlayer.MediaPlayer.Play();
            PlayButton.Icon = new SymbolIcon(Symbol.Pause);
            _isPlaying = true;
            StatusText.Text = "Now Playing: " + currentSong.name;
        }
    }
}
