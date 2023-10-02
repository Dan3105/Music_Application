from .models import CustomUser

class EmailAuthBackend:
    """
    Authenticate using backend
    """
    def authenticate(self, request, email=None, password=None):
        try:
            user = CustomUser.objects.get(email=email)
            if user.check_password(password):
                return user
            return None
        
        except (CustomUser.DoesNotExist, CustomUser.MultipleObjectsReturned):
            return None
    """
    user_name <=> user_id
    """    
    def get_user(self, user_name):
        try:
            return CustomUser.objects.get(pk=user_name)
        except CustomUser.DoesNotExist:
            return None