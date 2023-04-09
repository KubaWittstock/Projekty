from operator import ge
import re
from tkinter.messagebox import NO
from traceback import print_tb
from django.views import generic
from matplotlib import artist
from requests import Request, api, post
from rest_framework.response import Response
from django.shortcuts import redirect
from rest_framework import status, generics
from rest_framework.views import APIView
from .credentials import *
from .utils import *
from .models import *
from .serializer import *
import json
from requests import post
import random
import locale

locale.setlocale(locale.LC_COLLATE, ('pl_PL', 'utf-8'))
class TopItemsType:
    ARTISTS = "artists"
    TRACKS = "tracks"


class TokensView(generics.ListAPIView):
    queryset = SpotifyToken.objects.all()
    serializer_class = SpotifyTokenSerializer

def DeleteAllTokens(request):
    queryset = SpotifyToken.objects.all()
    if len(queryset) > 0:
        queryset.delete()
    return redirect('/spotify_backend/tokens')

class AuthURL(APIView):
    def get(self, request, format=None):
        scope = 'user-read-private, user-read-email, user-top-read, playlist-modify-public, playlist-modify-private'
        print(scope)
        url = Request('GET', 'https://accounts.spotify.com/authorize', params={
            'scope': scope,
            'response_type': 'code',
            'redirect_uri': REDIRECT_URI,
            'client_id': CLIENT_ID,
        }).prepare().url

        return Response({'url': url}, status=status.HTTP_200_OK)

class TestURL(APIView):
    def get(self, request, format=None):
        scope = 'user-read-private, user-read-email'
        print(scope)
        url = Request('GET', 'https://accounts.spotify.com/authorize', params={
            'scope': scope,
            'response_type': 'code',
            'redirect_uri': REDIRECT_URI,
            'client_id': CLIENT_ID,
        }).prepare().url

        return Response({'url': url}, status=status.HTTP_200_OK)


def spotify_callback(request, format=None):

    code = request.GET.get('code')
    error = request.GET.get('error')

    response = post('https://accounts.spotify.com/api/token', data={
        'grant_type': 'authorization_code',
        'code': code,
        'redirect_uri': REDIRECT_URI,
        'client_id': CLIENT_ID,
        'client_secret': CLIENT_SECRET
    }).json()

    access_token = response.get('access_token')
    token_type= response.get('token_type')
    refresh_token = response.get('refresh_token')
    expires_in = response.get('expires_in')
    if not request.session.exists(request.session.session_key):
        request.session.create()

    update_or_create_user_token(request.session.session_key, access_token, token_type, expires_in, refresh_token)

    return redirect('frontend:')



class IsAuthenticated(APIView):
    def get(self, request, format=None):
        is_authenticated = is_spotify_authenticated(self.request.session.session_key)
        return Response({'status': is_authenticated}, status=status.HTTP_200_OK)


class UserProfile(APIView):
    def get(self, request, format=None):
        endpoint = "v1/me"
        token = get_user_token(request.session.session_key)
        if token != None:
            response = execute_spotify_api_request(token.access_token, endpoint)
            response = response.get('response')
            if 'error' in response:
                return Response({'response': {}}, status=status.HTTP_204_NO_CONTENT)
            display_name = response.get('display_name')
            return Response({'user_name': display_name}, status=status.HTTP_200_OK)
        else:
            return Response({'response': {}}, status=status.HTTP_403_FORBIDDEN)

class UserTopItems(APIView):

    def sort_by_name(self, d):
         return locale.strxfrm(d.get('name').lower())

    def get(self, request, format=None):
        token = get_user_token(request.session.session_key)
        if token != None:
            artists = self.get_artists(token.access_token)
            tracks = self.get_tracks(token.access_token)
            generes = self.get_generes(artists)
            return Response({'artists': artists, 'tracks': tracks, 'genres': generes}, status=status.HTTP_200_OK)            
        else:
            return Response({'response': {}}, status=status.HTTP_403_FORBIDDEN)

    def get_top_items(self, item, access_token):
        endpoint = f"v1/me/top/{item}?time_range=long_term&limit=50&offset=0"
        response = execute_spotify_api_request(access_token, endpoint)
        response = response.get('response')

        return response

    def get_artists(self, access_token):
        response = self.get_top_items("artists", access_token)

        artists = []
        elements = response.get('items')
        for artist in elements:
            artist_name = artist.get('name')
            artist_imgs = artist.get('images')
            if len(artist_imgs) > 0:
                artist_img = artist_imgs[0].get('url')
            else:
                artist_img = ''
            artist_uri = artist.get('uri')
            artist_genres = artist.get('genres')
            artists.append({
                'name': artist_name,
                'img': artist_img,
                'uri': artist_uri,
                'genres': artist_genres,
            })
        return sorted(artists, key=self.sort_by_name)


    def get_tracks(self, access_token):
        response = self.get_top_items("tracks", access_token)

        tracks = []
        elements = response.get('items')
        for track in elements:
            track_name = track.get('name')
            track_album = track.get('album')
            track_imgs = track_album.get('images')
            if len(track_imgs) > 0:
                track_img = track_imgs[0].get('url')
            else:
                track_img = ''
            track_uri = track.get('uri')
            tracks.append({
                'name': track_name,
                'img': track_img,
                'uri': track_uri
            })
        return sorted(tracks, key=self.sort_by_name)
    
    def get_generes(self, artists):
        genres = []
        for genres_list in artists:
            for genre in genres_list['genres']:
                for word in genre.split(" "):
                    if word in GENRES and word not in genres:
                        genres.append(word)
        genres.sort()
        res = []
        for genre in genres:
            res.append({'name': genre, 'uri': genre})
        return res

class CreatePlaylist(APIView):
    def get(self, request, format=None, **kwargs,):
        token = get_user_token(request.session.session_key)
        artists = kwargs.get("artists") if kwargs.get("artists") else ""
        tracks = kwargs.get("tracks") if kwargs.get("tracks") else ""
        genres = kwargs.get("genres") if kwargs.get("genres") else ""
        noTracks = int(kwargs.get("noTracks"))
        addSet = True if kwargs.get("addSet") == "true" else False
        pValence = int(kwargs.get("pValence")) / 100
        pEnergy = int(kwargs.get("pEnergy")) / 100


        seeds = []
        if artists != None:
            for a in artists.split(','):
                seeds.append({'uri': a, 'type': 'a'})
        if tracks != None:
            for t in tracks.split(','):
                seeds.append({'uri': t, 'type': 't'})
        if genres != None:
            for g in genres.split(','):
                seeds.append({'uri': g, 'type': 'g'})

        
        chunks = list(create_chunks(random.sample(seeds, len(seeds)), 5))
        recomended_tracks = []
        for chunk in chunks:
            artists_c = []
            tracks_c = []
            genres_c = []
            for elem in chunk:
                if elem['type'] == 'a':
                   artists_c.append(elem['uri'])
                if elem['type'] == 't':
                   tracks_c.append(elem['uri'])
                if elem['type'] == 'g':
                   genres_c.append(elem['uri'])

            artists_c = ','.join(artists_c)
            tracks_c = ','.join(tracks_c)
            genres_c = ','.join(genres_c)

            for x in get_recomendations(token.access_token, 100, addSet, pValence, pEnergy, seed_artists=artists_c, seed_genres=genres_c, seed_tracks=tracks_c):
                recomended_tracks.append(x)
        
        recomended_tracks = list(set(recomended_tracks))
        random.shuffle(recomended_tracks)
        recomended_tracks = recomended_tracks[:noTracks]

        artists_names = get_items_name(token.access_token, "artists", artists)
        tracks_names = get_items_name(token.access_token, "tracks", tracks)
        genres_names = []
        if genres != "":
            for g in genres.split(","):
                genres_names.append(g)
        
        artists_names_str = get_description_details(artists_names, "artist")
        tracks_names_str = get_description_details(tracks_names, "track") 
        genres_names_str = get_description_details(genres_names, "genre") 

        items_str = [x for x in [artists_names_str, tracks_names_str, genres_names_str] if x != ""]
        items_str = ", ".join(items_str)

        name = get_playlist_name(addSet, pEnergy, pValence, "settings")
        description = f'Inspired by {items_str}.'

        if addSet:
            description += f" Playlist settings: energetic {int(pEnergy * 100)}%, upbeat {int(pValence * 100)}%"
        response = create_playlist(token, name, description, recomended_tracks)
        return response

        
class GetTracksFromPlaylist(APIView):
    def get(self, request, playlist_id, noTracks, addSet, pValence, pEnergy, format=None):
        addSet = True if addSet == 1 else False
        pValence = int(pValence) / 100
        pEnergy = int(pEnergy) / 100
        endpoint = f"v1/playlists/{playlist_id}"
        token = get_user_token(request.session.session_key)
        if token != None:
            response = execute_spotify_api_request(token.access_token, endpoint)
            if 'Error' in response:
                return Response({}, status=status.HTTP_200_OK)
            elif response['status_code'] == status.HTTP_404_NOT_FOUND:
                return Response({'error': "Playlist not found"}, status=status.HTTP_200_OK)
            else:
                response = response.get('response')
                items = response.get('tracks').get('items')
                tracks_uri = []
                for track in items:
                    t = track.get('track')
                    track_uri = t.get('uri')
                    track_uri = track_uri.split(":")[-1]
                    tracks_uri.append(track_uri)
                author_display_name = response.get('owner').get('display_name')
                playlist_name = response.get('name')
                if len(tracks_uri) > 5:
                    chunks = create_chunks(tracks_uri, 5)
                else:
                    chunks = [tracks_uri]
                recomended_tracks = []

                print(tracks_uri, chunks)
                for chunk in chunks:
                    print(f"C: {chunk}")
                    tracks = ",".join(chunk)
                    for x in get_recomendations(token.access_token, 100, addSet, pValence, pEnergy, seed_tracks=tracks):
                        recomended_tracks.append(x)
                
                recomended_tracks = list(set(recomended_tracks))
                random.shuffle(recomended_tracks)
                recomended_tracks = recomended_tracks[:noTracks]
                name = get_playlist_name(addSet, pEnergy, pValence, "playlist")
                description = f'Inspired by playlist "{playlist_name}" by {author_display_name}.'
                if addSet:
                    description += f" Playlist settings: energetic {int(pEnergy * 100)}%, upbeat {int(pValence * 100)}%"

                response = create_playlist(token, name, description, recomended_tracks)
                return response

        return Response({}, status=status.HTTP_403_FORBIDDEN)