from rest_framework import routers
from core.user.viewsets import UserViewSet
from core.authentication.viewsets import LoginViewSet, RegisterViewSet, RefreshViewSet
from core.playlist.viewsets import PlaylistViewSet
router = routers.SimpleRouter()

###
# USER
# 
# ###

router.register(r'user', UserViewSet, basename='user')

##
# AUTH
# ###
router.register(r'auth/register', RegisterViewSet, basename='auth-register')
router.register(r'auth/login', LoginViewSet, basename='auth-login')
router.register(r'auth/refresh', RefreshViewSet, basename='auth-refresh')
router.register(r'playlist', PlaylistViewSet, basename='playlist')
urlpatterns = [
    *router.urls,
]
