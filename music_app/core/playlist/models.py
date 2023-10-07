from django.db import models
from django.db.models.query import QuerySet
from core.abstract.model import AbstractModel, AbstractManager
# Create your models here.

class PlaylistManager(AbstractManager):
    def all(self):
        return super().all()
    pass

class Playlist(AbstractModel):
    objects = PlaylistManager()

    user = models.ForeignKey(to='core_user.User', on_delete=models.CASCADE)
    title = models.TextField(null=False, blank=False)
    description = models.CharField(null=True, blank=False)

    
    def __str__(self) -> str:
        return self.title
    
    class Meta:
        db_table = "'core.playlist'"