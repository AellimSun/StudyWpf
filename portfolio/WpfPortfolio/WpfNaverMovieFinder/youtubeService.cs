using Google.Apis.Services;

namespace WpfNaverMovieFinder
{
    internal class youtubeService
    {
        private BaseClientService baseClientService;

        public youtubeService(BaseClientService baseClientService)
        {
            this.baseClientService = baseClientService;
        }
    }
}