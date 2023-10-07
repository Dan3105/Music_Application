from rest_framework import serializers
from rest_framework.exceptions import ValidationError

from core.abstract.serializers import AbstractSerializer
from core.user.models import User
from core.playlist.models import Playlist

class PlaylistSerializers(AbstractSerializer):
    user = serializers.SlugRelatedField(queryset=User.objects.all(), slug_field='public_id')
    
    def validate_user(self, value):
        if self.context['request'].user != value:
            raise ValidationError("You can't create a playlist for another user")
        return value
    
    def update(self, instance, validated_data):
        #instance here is playlist reader
        #validated_data dictionary data
        #duma lạ vậy ta :)
        return super().update(instance, validated_data)
    
    class Meta:
        model = Playlist
        # List of all the fields
        fields = ['id', 'user', 'title', 'description', 'created', 'updated']
            