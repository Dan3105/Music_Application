from django.db import models
import uuid
from django.contrib.auth.models import AbstractBaseUser, BaseUserManager, PermissionsMixin
from django.core.exceptions import ObjectDoesNotExist
from django.db import models
from django.http import Http404
from core.abstract.model import AbstractManager, AbstractModel
# Create your models here.

class UserManager(BaseUserManager, AbstractManager):
    def create_user(self, email, password=None, **kwargs):
        if email is None:
            raise TypeError('User must have email')
        if password is None:
            raise TypeError('Password must have email')
        
        user = self.model(email=self.normalize_email(email), **kwargs)
        user.set_password(password)
        user.save(using=self._db)

        return user
    
    def create_superuser(self, email, password=None, **kwargs):
        user = self.create_user(email, password)
        user.is_superuser=True
        user.is_staff=True
        user.save(using=self._db)

        return user

class User(AbstractModel, AbstractBaseUser, PermissionsMixin):
    email = models.EmailField(db_index=True, unique=True)
    
    is_active = models.BooleanField(default=True)
    is_superuser = models.BooleanField(default=False)

    USERNAME_FIELD = 'email'
    REQUIRED_FIELDS = []

    objects = UserManager()

    def __str__(self):
        return f'{self.email}'
    