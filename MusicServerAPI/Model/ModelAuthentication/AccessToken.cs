﻿namespace MusicServerAPI.Model.ModelAuthentication
{
    public class AccessToken
    {
        public string? Token { set; get; }
        public DateTime Created { set; get; }
        public DateTime Expired { set; get; }
    }
}
