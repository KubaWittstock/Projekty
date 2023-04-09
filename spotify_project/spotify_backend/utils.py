from .models import SpotifyToken
from django.utils import timezone
from rest_framework.response import Response
from rest_framework import status
from datetime import timedelta
from requests import post
from requests.api import get, put
from .credentials import CLIENT_ID, CLIENT_SECRET
import json

BASE_URL = "https://api.spotify.com/"
GENRES = ["acoustic", "afrobeat", "alt-rock", "alternative", "ambient", "anime", "black-metal", "bluegrass", "blues", "bossanova", "brazil", "breakbeat", "british", "cantopop", "chicago-house",
            "children", "chill", "classical", "club", "comedy", "country", "dance", "dancehall", "death-metal", "deep-house", "detroit-techno", "disco", "disney", "drum-and-bass", "dub", "dubstep",
            "edm", "electro", "electronic", "emo", "folk", "forro", "french", "funk", "garage", "german", "gospel", "goth", "grindcore", "groove", "grunge", "guitar", "happy", "hard-rock", "hardcore",
            "hardstyle", "heavy-metal", "hip-hop", "holidays", "honky-tonk", "house", "idm", "indian", "indie", "indie-pop", "industrial", "iranian", "j-dance", "j-idol", "j-pop", "j-rock", "jazz",
            "k-pop", "kids", "latin", "latino", "malay", "mandopop", "metal", "metal-misc", "metalcore", "minimal-techno", "movies", "mpb", "new-age", "new-release", "opera", "pagode", "party",
            "philippines-opm", "piano", "pop", "pop-film", "post-dubstep", "power-pop", "progressive-house", "psych-rock", "punk", "punk-rock", "r-n-b", "rainy-day", "reggae", "reggaeton", "road-trip",
            "rock", "rock-n-roll", "rockabilly", "romance", "sad", "salsa", "samba", "sertanejo", "show-tunes", "singer-songwriter", "ska", "sleep", "songwriter", "soul", "soundtracks", "spanish", "study",
            "summer", "swedish", "synth-pop", "tango", "techno", "trance", "trip-hop", "turkish", "work-out", "world-music"]


def get_user_token(session_id):
    user_tokens = SpotifyToken.objects.filter(user=session_id)
    if user_tokens.exists():
        return user_tokens[0]
    return None

def is_spotify_authenticated(session_id):
    token = get_user_token(session_id)
    if token:
        expiry = token.expires_in
        if expiry <= timezone.now():
            refresh_spotify_token(session_id)                  
        return True     
    return False

def refresh_spotify_token(session_id):
    refresh_token = get_user_token(session_id).refresh_token
    print("Refreshing token :)")
    response = post('https://accounts.spotify.com/api/token', data={
        'grant_type': 'refresh_token',
        'refresh_token': refresh_token,
        'client_id': CLIENT_ID,
        'client_secret': CLIENT_SECRET,
    }).json()

    access_token = response.get('access_token')
    token_type = response.get('token_type')
    expires_in = response.get('expires_in')

    update_or_create_user_token(session_id, access_token, token_type, expires_in, refresh_token)

def update_or_create_user_token(session_id, access_token, token_type, expires_in, refresh_token):
    token = get_user_token(session_id=session_id)
    expires_in = timezone.now() + timedelta(seconds=expires_in)

    if token:
        token.access_token = access_token
        token.refresh_token = refresh_token
        token.expires_in = expires_in
        token.token_type = token_type

        token.save(update_fields=['access_token', 'refresh_token', 'expires_in', 'token_type'])
    else:
        token = SpotifyToken(user=session_id, access_token=access_token, refresh_token=refresh_token, token_type=token_type, expires_in=expires_in)
        token.save()

def execute_spotify_api_request(access_token, endpoint, data = None, post_=False, put_=False):
    headers = {'Content-Type': 'application/json', 'Authorization': "Bearer " + access_token}
    
    if post_:
        response = post(url=BASE_URL + endpoint, data=data, headers=headers)
    elif put_:
        response = put(url=BASE_URL + endpoint, headers=headers)
    else:
        response = get(url=BASE_URL + endpoint, headers=headers)
        print(response)
    try:
        return {'response': response.json(), 'status_code': response.status_code}
    except Exception as e:
        return {'Error': f'Issue with request {e}'}

def get_recomendations(token, n, addSet=False, pValence=0.5, pEnergy=0.5, **kwargs):
    # Prepare query
    query = ''.join([f"&{key}={value}" for key, value in kwargs.items()])
        
    addQuery = f"&target_valence={pValence}&target_energy={pEnergy}"

    # Prepare endpoint and execute request
    endpoint = f"v1/recommendations?limit={n}{query}"
    if addSet:
        endpoint += addQuery
    print(endpoint)
    response = execute_spotify_api_request(token, endpoint)

    # Grab list of recommended tracks' ids
    tracks = response.get('response').get('tracks')
    ids = [t.get('id') for t in tracks]
    return ids

def get_playlist_name(addSet, energy, valence, created_with):
    name = f"Perfect Playlist (created with {created_with})"
    if addSet:
        energy = "Energetic" if energy > 0.5 else "Mellow"
        upbeat = "Upbeat" if valence > 0.5 else "Downbeat"
        name += f" - {energy} & {upbeat}"

    return name

def get_description_details(item, item_name):
    if len(item) == 1:
        return f"{item_name}: " + ", ".join(item)
    elif len(item) > 1:
        return f"{item_name}s: " + ", ".join(item)
    else:
       return ""


def get_items_name(token, item_type, ids):
    names = []
    if ids != "": 
        for id in ids.split(","):
            endpoint = f"v1/{item_type}/{id}"
            response = execute_spotify_api_request(token, endpoint)
            name = response.get('response').get('name')
            names.append(name)
    return names

def create_playlist(token, name, description, tracks):
    if token != None:
        endpoint = "v1/me"
        response = execute_spotify_api_request(token.access_token, endpoint)
        user_id = response.get('response').get('id')
        endpoint = f"v1/users/{user_id}/playlists"
        
        playlist_props = json.dumps({
            "name": name,
            "description": description,
            "public": False
        })
        response = execute_spotify_api_request(token.access_token, endpoint, playlist_props, post_=True)
            
        if response.get('status_code') == status.HTTP_201_CREATED:                
            response = response.get('response')
            playlist_id = response.get('id')
            endpoint = f"v1/playlists/{playlist_id}/tracks"
            tracks_with_badge = ['spotify:track:' + track for track in tracks]
            songs_to_add = json.dumps({
                "uris": tracks_with_badge                 
            })

            response = execute_spotify_api_request(token.access_token, endpoint, songs_to_add, post_=True)
            return Response({}, status=response.get('status_code'))
        else:
            return Response({}, status=response.get('status_code'))
    else:
        return Response({}, status=status.HTTP_403_FORBIDDEN)

def create_chunks(list_name, n):
    for i in range(0, len(list_name), n):
        yield list_name[i:i + n]
        