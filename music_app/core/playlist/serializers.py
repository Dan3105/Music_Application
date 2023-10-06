from rest_framework import serializers
from rest_framework.exceptions import ValidationError

from core.abstract.serializers import AbstractSerializer
from core.user.models import User
from core.playlist.models import Playlist

class PlaylistSerializers(AbstractSerializer):
    user = serializers.SlugRelatedField(queryset=User.objects.all(), slug_field='public_id')

    def validate_author(self, value):
        if self.context['request'].user != value:
            raise ValidationError("You can't create a playlist for another user")
        return value
    
    class Meta:
        model = Playlist
        # List of all the fields
        fields = ['id', 'user', 'title', 'description', 'created', 'updated']
            