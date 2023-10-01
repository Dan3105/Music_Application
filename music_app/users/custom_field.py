import hashlib
import uuid
from django.db import models

class HashedPrimaryKeyField(models.Field):
    def __init__(self, *args, **kwargs):
        kwargs['editable'] = False
        kwargs['unique'] = True
        kwargs['default'] = self.generate_hash
        kwargs['max_length'] = 32  # Adjust max_length as needed
        super().__init__(*args, **kwargs)

    def generate_hash(self):
        return hashlib.sha256(uuid.uuid4().hex.encode('utf-8')).hexdigest()

    def db_type(self, connection):
        return 'char(%s)' % self.max_length
