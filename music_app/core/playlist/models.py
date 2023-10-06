from django.db import models
from core.abstract.model import AbstractModel, AbstractManager
# Create your models here.

class PlaylistManager(AbstractManager):
    pass

class Playlist(AbstractModel):
    user = models.ForeignKey(to='core_user.User', on_delete=models.CASCADE)
    title = models.TextField(null=True, blank=False)
    description = models.CharField(null=False, blank=False)

    objects = PlaylistManager()

    def __str__(self) -> str:
        return self.title
    
    class Meta:
        db_table = "'core.playlist'"