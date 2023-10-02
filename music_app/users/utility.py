import hashlib
import uuid
from django.db import models

def generate_hash():
    return hashlib.sha256(uuid.uuid4().hex.encode('utf-8')).hexdigest()