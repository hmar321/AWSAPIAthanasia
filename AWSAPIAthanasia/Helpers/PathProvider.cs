using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;

namespace ApiAthanasia.Helpers
{
    public enum FoldersImages { Portadas = 1, Usuarios = 2 }
    public class HelperPathProvider
    {
        private IServer server;
        private IWebHostEnvironment hostEnvironment;
        public HelperPathProvider(IServer server, IWebHostEnvironment hostEnvironment)
        {
            this.server = server;
            this.hostEnvironment = hostEnvironment;
        }
        public string MapUrlServerPath()
        {
            var addresses = server.Features.Get<IServerAddressesFeature>().Addresses;
            string serverUrl = addresses.FirstOrDefault();
            return serverUrl;
        }

        private string GetFolderPath(FoldersImages folder)
        {
            string carpeta = "";
            if (folder == FoldersImages.Portadas)
            {
                carpeta = "images/portadas";
            }else if (folder == FoldersImages.Usuarios)
            {
                carpeta = "images/usuarios";
            }
            return carpeta;
        }

        public string MapPath(string fileName, FoldersImages folder)
        {
            string carpeta = this.GetFolderPath(folder);
            string rootPath = this.hostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath, carpeta, fileName);
            return path;
        }

        public string MapUrlPath(string fileName, FoldersImages folder)
        {
            string carpeta = this.GetFolderPath(folder);
            var addresses = server.Features.Get<IServerAddressesFeature>().Addresses;
            string serverUrl = addresses.FirstOrDefault();
            string urlPath = serverUrl + "/" + carpeta + "/" + fileName;
            return urlPath;
        }
    }
}
