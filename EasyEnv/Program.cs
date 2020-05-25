namespace EasyEnv
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
            ################################################################################################
            ## PARA GERAR/RODAR IMAGEM
            ## docker build -t sqlimagefromdockerfile .
            ## docker run -p 1433:1433 --name SQLfromDockerFile -d sqlimagefromdockerfile

            ## PARA SALVAR/CARREGAR IMAGEM:
            ## docker save -o sqlimagefromdockerfile.tar sqlimagefromdockerfile
            ## docker load -i sqlimagefromdockerfile.tar

            ##PARA REMOVER CONTEINER/IMAGEM:
            ## docker stop SQLfromDockerFile
            ## docker rm SQLfromDockerFile
            ## docker image rm sqlimagefromdockerfile
            ################################################################################################
             */
            var docker = new Docker(@"C:\test_docker", new ConsoleNotifier());

            //docker build -t sqlimagefromdockerfile .
            docker.BuildAnImage("sqlimage", "./", "sqlimage.dockerfile");

            // run -p 1433:1433 --name SQLfromDockerFile -d sqlimagefromdockerfile
            docker.RunConteiner("sqlConteiner", "sqlimage", 1433, 1433);


            var manager = new ContainerManager();
            manager.Execute();
        }
    }
}
