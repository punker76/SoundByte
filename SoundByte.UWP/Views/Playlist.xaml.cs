﻿/* |----------------------------------------------------------------|
 * | Copyright (c) 2017, Grid Entertainment                         |
 * | All Rights Reserved                                            |
 * |                                                                |
 * | This source code is to only be used for educational            |
 * | purposes. Distribution of SoundByte source code in             |
 * | any form outside this repository is forbidden. If you          |
 * | would like to contribute to the SoundByte source code, you     |
 * | are welcome.                                                   |
 * |----------------------------------------------------------------|
 */

using Windows.UI.Xaml.Navigation;
using SoundByte.Core.Services;
using SoundByte.UWP.Services;

namespace SoundByte.UWP.Views
{
    /// <summary>
    /// Displays a playlist
    /// </summary>
    public sealed partial class Playlist
    {
        // Page View Model
        public readonly ViewModels.PlaylistViewModel ViewModel = new ViewModels.PlaylistViewModel();

        public Playlist()
        {
            // Setup the XAML
            InitializeComponent();
            // This page must be cached
            NavigationCacheMode = NavigationCacheMode.Enabled;
            // Set the data context
            DataContext = ViewModel;
        }

        /// <summary>
        /// Called when the user navigates to the page
        /// </summary>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // Make sure the view is ready for the user
            await ViewModel.SetupView(e.Parameter as Core.API.Endpoints.Playlist);
            // Track Event
            TelemetryService.Current.TrackPage("Playlist Page");
        }
    }
}
