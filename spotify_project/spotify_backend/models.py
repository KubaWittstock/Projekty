from django.db import models


class SpotifyToken(models.Model):
    user = models.CharField(max_length=50, unique=True)
    create_at = models.DateTimeField(auto_now_add=True)
    refresh_token = models.CharField(max_length=150, unique=True)
    access_token = models.CharField(max_length=150, unique=True)
    expires_in = models.DateTimeField()
    token_type = models.CharField(max_length=50)
