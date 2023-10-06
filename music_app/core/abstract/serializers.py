from rest_framework import serializers
class AbstractSerializer(serializers.Serializer):
    id = serializers.UUIDField(source='public_id', read_only=True, format='hex')
    updated = serializers.DateTimeField(read_only=True)
    created = serializers.DateTimeField(read_only=True)
