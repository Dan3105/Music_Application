from django.urls import path
from django.contrib.auth import views as auth_view
from .views import user_profile

urlpatterns=[
    #Log in/Log out
    path('login/', auth_view.LoginView.as_view(), name='login'),
    path('logout/', auth_view.LogoutView.as_view(), name='logout'),
    
    #Change Password
    path('password-change/', auth_view.PasswordChangeView.as_view(), 
         name='password_change'),
    
    #Default account
    path('', user_profile, name='profile'),
]