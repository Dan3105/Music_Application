from django.db import models
from django.contrib.auth.models import AbstractUser
from .custom_field import HashedPrimaryKeyField

# Create your models here.
class CustomUser(AbstractUser):
    USERNAME_FIELD = 'email'
    REQUIRED_FIELDS = []

    email = models.EmailField(primary_key=True)
    username = HashedPrimaryKeyField()
    
