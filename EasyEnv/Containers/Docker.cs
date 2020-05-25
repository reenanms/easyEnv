namespace EasyEnv
{
    public class Docker
    {

        /* SQL SERVER DOCKERFILE SAMPLE:
        
        FROM mcr.microsoft.com/mssql/server:2017-latest

            ENV ACCEPT_EULA=Y
            ENV SA_PASSWORD=Test@12345
            ENV MSSQL_PID=Developer
            ENV MSSQL_TCP_PORT=1433 

        WORKDIR /backup

        COPY db.bak db.bak
        COPY restoredb.sql restoredb.sql


        RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" &&  /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P Test@12345 -i restoredb.sql 
        RUN rm db.bak restoredb.sql



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

        private const string PROGRAM_NAME = "docker";

        public Docker(string workingDirectory, INotifier notifier)
        {
            WorkingDirectory = workingDirectory;
            Notifier = notifier;
        }

        public string WorkingDirectory { get; private set; }
        public INotifier Notifier { get; private set; }

        public void BuildAnImage(string newImageName, string configFilePath, string configFileName)
        {
            //docker build -t "sqlimage:1.0" -f "sqlimage.dockerfile" "./"
            run("build", $"-t \"{newImageName}\"", $"-f \"{configFileName}\"", configFilePath);
        }

        public void RunConteiner(string conteinerName, string imageName, int outputPort, int exposePort)
        {
            //run -p 1433:1433 --name SQLfromDockerFile -d sqlimagefromdockerfile
            run("run", $"-p {outputPort}:{exposePort}", $"--name \"{conteinerName}\"", $"-d \"{imageName}\"");
        }

        public void StopConteiner()
        {

        }

        public void RemoveConteiner()
        {

        }

        public void RemoveImage()
        {

        }

        public void SaveImageInFile()
        {

        }

        public void LoadImageFromFile()
        {

        }

        private string run(params string[] arguments)
        {
            var program = new Executetor(PROGRAM_NAME, WorkingDirectory)
            {
                Notifier = Notifier
            };

            return program.Run(arguments);
        }
    }
}
