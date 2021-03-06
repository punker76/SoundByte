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

using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.Xaml.Controls;
using SoundByte.Core;
using SoundByte.Core.Items.Playlist;
using SoundByte.Core.Items.Track;
using SoundByte.Core.Items.User;
using SoundByte.Core.Services;
using SoundByte.Core.Sources.SoundCloud;
using SoundByte.UWP.Converters;
using SoundByte.UWP.Helpers;
using SoundByte.UWP.Services;
using SoundByte.UWP.Views;

namespace SoundByte.UWP.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private string _followUserIcon = "\uE8FA";
        private string _followUserText;

        // Icon for the pin button
        private string _pinButtonIcon = "\uE718";

        // Text for the pin button
        private string _pinButtonText;

        // The current pivot item
        private PivotItem _selectedPivotItem;

        private bool _showFollowButton = true;
        private BaseUser _user;

        public bool ShowFollowButton
        {
            get => _showFollowButton;
            set
            {
                if (value != _showFollowButton)
                {
                    _showFollowButton = value;
                    UpdateProperty();
                }
            }
        }

        /// <summary>
        ///     The current pivot item that the user is viewing
        /// </summary>
        public PivotItem SelectedPivotItem
        {
            get => _selectedPivotItem;
            set
            {
                if (value != _selectedPivotItem)
                {
                    _selectedPivotItem = value;
                    UpdateProperty();
                }
            }
        }

        public string PinButtonIcon
        {
            get => _pinButtonIcon;
            set
            {
                if (value != _pinButtonIcon)
                {
                    _pinButtonIcon = value;
                    UpdateProperty();
                }
            }
        }

        public string FollowUserIcon
        {
            get => _followUserIcon;
            set
            {
                if (value != _followUserIcon)
                {
                    _followUserIcon = value;
                    UpdateProperty();
                }
            }
        }

        public string FollowUserText
        {
            get => _followUserText;
            set
            {
                if (value != _followUserText)
                {
                    _followUserText = value;
                    UpdateProperty();
                }
            }
        }

        public string PinButtonText
        {
            get => _pinButtonText;
            set
            {
                if (value != _pinButtonText)
                {
                    _pinButtonText = value;
                    UpdateProperty();
                }
            }
        }

        public BaseUser User
        {
            get => _user;
            private set
            {
                if (value == _user) return;

                _user = value;
                UpdateProperty();
            }
        }

        /// <summary>
        ///     Setup this viewmodel for a specific user
        /// </summary>
        /// <param name="user"></param>
        public async Task UpdateModel(BaseUser user)
        {
            // Set the new user
            User = user;

            // Set the models
            TracksList.Source.User = user;
            TracksList.RefreshItems();

            LikeItems.Source.User = user;
            LikeItems.RefreshItems();

            PlaylistItems.Source.User = user;
            PlaylistItems.RefreshItems();

            FollowersList.Source.User = user;
            FollowersList.Source.Type = "followers";
            FollowersList.RefreshItems();

            FollowingsList.Source.User = user;
            FollowingsList.Source.Type = "followings";
            FollowingsList.RefreshItems();

            // Get the resource loader
            var resources = ResourceLoader.GetForCurrentView();

            // Check if the tile has been pinned
            if (TileHelper.IsTilePinned("User_" + User.Id))
            {
                PinButtonIcon = "\uE77A";
                PinButtonText = resources.GetString("AppBarUI_Unpin_Raw");
            }
            else
            {
                PinButtonIcon = "\uE718";
                PinButtonText = resources.GetString("AppBarUI_Pin_Raw");
            }

            if (SoundByteV3Service.Current.IsServiceConnected(ServiceType.SoundCloud) &&
                User.Id == SoundByteV3Service.Current.GetConnectedUser(ServiceType.SoundCloud)?.Id)
            {
                ShowFollowButton = false;
            }
            else
            {
                ShowFollowButton = true;

                // Check if we are following the user
                if (await SoundByteV3Service.Current.ExistsAsync(ServiceType.SoundCloud, "/me/followings/" + User.Id))
                {
                    FollowUserIcon = "\uE1E0";
                    FollowUserText = "Unfollow User";
                }
                else
                {
                    FollowUserIcon = "\uE8FA";
                    FollowUserText = "Follow User";
                }
            }

            if (SoundByteV3Service.Current.IsServiceConnected(ServiceType.Fanburst) &&
                User.Id == SoundByteV3Service.Current.GetConnectedUser(ServiceType.Fanburst)?.Id)
            {
                ShowFollowButton = false;
            }
            else
            {
                ShowFollowButton = true;

                // Check if we are following the user
                if (await SoundByteV3Service.Current.ExistsAsync(ServiceType.Fanburst, "/me/following/" + User.Id))
                {
                    FollowUserIcon = "\uE1E0";
                    FollowUserText = "Unfollow User";
                }
                else
                {
                    FollowUserIcon = "\uE8FA";
                    FollowUserText = "Follow User";
                }
            }
        }

        /// <summary>
        ///     Follows the requested user
        /// </summary>
        public async void FollowUser()
        {
            // Show the loading ring
            await App.SetLoadingAsync(true);

            // Check if we are following the user
            if (await SoundByteV3Service.Current.ExistsAsync(ServiceType.SoundCloud, "/me/followings/" + User.Id))
            {
                // Unfollow the user
                if (await SoundByteV3Service.Current.DeleteAsync(ServiceType.SoundCloud, "/me/followings/" + User.Id))
                {
                    TelemetryService.Instance.TrackEvent("Unfollow User");
                    FollowUserIcon = "\uE8FA";
                    FollowUserText = "Follow User";
                }
                else
                {
                    FollowUserIcon = "\uE1E0";
                    FollowUserText = "Unfollow User";
                }
            }
            else
            {
                // Follow the user
                if (await SoundByteV3Service.Current.PutAsync(ServiceType.SoundCloud, $"/me/followings/{User.Id}"))
                {
                    TelemetryService.Instance.TrackEvent("Follow User");
                    FollowUserIcon = "\uE1E0";
                    FollowUserText = "Unfollow User";
                }
                else
                {
                    FollowUserIcon = "\uE8FA";
                    FollowUserText = "Follow User";
                }
            }
            // Hide the loading ring
            await App.SetLoadingAsync(false);
        }

        public async void PinUser()
        {
            // Show the loading ring
            await App.SetLoadingAsync(true);
            // Get the resource loader
            var resources = ResourceLoader.GetForCurrentView();

            // Check if the tile exists
            if (TileHelper.IsTilePinned("User_" + User.Id))
            {
                // Try remove the tile
                if (await TileHelper.RemoveTileAsync("User_" + User.Id))
                {
                    TelemetryService.Instance.TrackEvent("Unpin User");
                    PinButtonIcon = "\uE718";
                    PinButtonText = resources.GetString("AppBarUI_Pin_Raw");
                }
                else
                {
                    PinButtonIcon = "\uE77A";
                    PinButtonText = resources.GetString("AppBarUI_Unpin_Raw");
                }
            }
            else
            {
                // Create the tile
                if (await TileHelper.CreateTileAsync("User_" + User.Id, User.Username,
                    "soundbyte://core/user?id=" + User.Id, new Uri(ArtworkConverter.ConvertObjectToImage(User)),
                    ForegroundText.Light))
                {
                    TelemetryService.Instance.TrackEvent("Pin User");
                    PinButtonIcon = "\uE77A";
                    PinButtonText = resources.GetString("AppBarUI_Unpin_Raw");
                }
                else
                {
                    PinButtonIcon = "\uE718";
                    PinButtonText = resources.GetString("AppBarUI_Pin_Raw");
                }
            }
            // Hide the loading ring
            await App.SetLoadingAsync(false);
        }

        public async void NavigateToUserTrack(object sender, ItemClickEventArgs e)
        {
            await App.SetLoadingAsync(true);

            var startPlayback =
                await PlaybackService.Instance.StartModelMediaPlaybackAsync(TracksList, false, (BaseTrack) e.ClickedItem);
            if (!startPlayback.Success)
                await new MessageDialog(startPlayback.Message, "Error playing user track.").ShowAsync();

            await App.SetLoadingAsync(false);
        }

        public async void NavigateToLikedTrack(object sender, ItemClickEventArgs e)
        {
            await App.SetLoadingAsync(true);

            var startPlayback =
                await PlaybackService.Instance.StartModelMediaPlaybackAsync(LikeItems, false, (BaseTrack) e.ClickedItem);
            if (!startPlayback.Success)
                await new MessageDialog(startPlayback.Message, "Error playing liked user track.").ShowAsync();

            await App.SetLoadingAsync(false);
        }

        public void NavigateToPlaylist(object sender, ItemClickEventArgs e)
        {
            App.NavigateTo(typeof(PlaylistView), e.ClickedItem as BasePlaylist);
        }

        public void NavigateToUser(object sender, ItemClickEventArgs e)
        {
            App.NavigateTo(typeof(UserView), e.ClickedItem as BaseUser);
        }

        #region Models

        // Items that the user has liked
        public SoundByteCollection<LikeSoundCloudSource, BaseTrack> LikeItems { get; } =
            new SoundByteCollection<LikeSoundCloudSource, BaseTrack>();

        // Playlists that the user has liked / uploaded
        public SoundByteCollection<UserSoundCloudPlaylistSource, BasePlaylist> PlaylistItems { get; } =
            new SoundByteCollection<UserSoundCloudPlaylistSource, BasePlaylist>();

        // List of user followers
        public SoundByteCollection<UserSoundCloudFollowingsSource, BaseUser> FollowersList { get; } =
            new SoundByteCollection<UserSoundCloudFollowingsSource, BaseUser>();

        // List of user followings
        public SoundByteCollection<UserSoundCloudFollowingsSource, BaseUser> FollowingsList { get; } =
            new SoundByteCollection<UserSoundCloudFollowingsSource, BaseUser>();

        // List of users tracks
        public SoundByteCollection<TrackSoundCloudSource, BaseTrack> TracksList { get; } =
            new SoundByteCollection<TrackSoundCloudSource, BaseTrack>();
        #endregion
    }
}