from typing import Any
from django.db import models
from django.contrib.auth.models import AbstractUser, UserManager
from .utility import generate_hash
from django.conf import settings
#from .custom_field import HashedPrimaryKeyField

class CustomUserManager(UserManager):
    def create_user(self,  email: str | None = ..., password: str | None = ..., **extra_fields):
        if not email:
            raise ValueError(_("The email is not valid"))
        email = self.normalize_email(email)
        user = generate_hash()
        
        user = self.model(username=user, email=email, **extra_fields)
        user.set_password(password)

        return super().create_user(user, email, password, **extra_fields)

    def create_superuser(self, email: str | None, password: str | None, **extra_fields):
        user = self.create_user(email, password)
        user.is_staff = True
        user.is_superuser = True
        user.save()
        return user
    

class Profile(models.Model):
    user = models.OneToOneField(settings.AUTH_USER_MODEL, on_delete=models.CASCADE)
    date_of_birth = models.DateField(blank=True, null=True)
    photo = models.ImageField(upload_to='users/%Y/%m/%d/', blank=True)

    def __str__(self):
        return f'Profile {self.user.email}' 

# Create your models here.
class CustomUser(AbstractUser):
    USERNAME_FIELD = 'email'
    REQUIRED_FIELDS = []

    objects = CustomUserManager()

    email = models.EmailField(unique=True)
    username = models.CharField(primary_key=True, max_length=150, editable=False)
