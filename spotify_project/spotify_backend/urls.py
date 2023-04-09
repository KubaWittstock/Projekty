from django.urls import path, re_path, include
from .views import *

urlpatterns = [
    path('isAuthenticated', IsAuthenticated.as_view()),
    path('getAuthenticationURL', AuthURL.as_view()),
    path('redirect', spotify_callback),
    path('tokens', TokensView.as_view()),
    path('deleteTokens', DeleteAllTokens),
    path('getUserProfile', UserProfile.as_view()),
    path('getUserTopItems', UserTopItems.as_view()),
    re_path(r'^createPlaylist/(?P<artists>[^&]+)?(&(?P<tracks>[^&]+)?)?(&(?P<genres>[^&]+)?)?(&(?P<noTracks>[\w-]+)?)?(&(?P<addSet>[\w-]+)?)?(&(?P<pValence>[\w-]+)?)?(&(?P<pEnergy>[\w-]+)?)?', CreatePlaylist.as_view()),
    path('getTracksFromPlaylist/<int:noTracks>&<int:addSet>&<int:pValence>&<int:pEnergy>&<str:playlist_id>', GetTracksFromPlaylist.as_view()),
]