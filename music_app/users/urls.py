from django.urls import path, include
from django.contrib.auth import views as auth_view
from .views import user_profile, register, user_profile, dashboard

urlpatterns=[
    #Log in/Log out
    #path('login/', auth_view.LoginView.as_view(), name='login'),
    #path('logout/', auth_view.LogoutView.as_view(), name='logout'),
    
    #Change Password
    #path('password-change/', auth_view.PasswordChangeView.as_view(), 
    #     name='password_change'),
    
    #path('password-change/done/', auth_view.PasswordChangeDoneView.as_view(), name='password_change_done'),

    # path('password-reset/', auth_view.PasswordResetView.as_view(), name='password_reset'),
    # path('password-reset/done/', auth_view.PasswordResetDoneView.as_view(), name='password_reset_done'),

    # path('password-reset/<uidb64>/<token>/', auth_view.PasswordResetConfirmView.as_view(), name='password_reset_confirm'),
    # path('password-reset/complete/', auth_view.PasswordResetCompleteView.as_view(), name='password_reset_complete'),

    path('', include('django.contrib.auth.urls')),

    path('', dashboard, name='dashboard'),
    path('profile/', user_profile, name='profile'),

    path('register/', register, name='register'),
]