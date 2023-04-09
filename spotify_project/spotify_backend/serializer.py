from django.db.models import fields
from rest_framework import serializers
from .models import SpotifyToken

class SpotifyTokenSerializer(serializers.ModelSerializer):
    class Meta:
        model = SpotifyToken
        fields = ('user', 'create_at', 'refresh_token', 'access_token', 'expires_in', 'token_type')