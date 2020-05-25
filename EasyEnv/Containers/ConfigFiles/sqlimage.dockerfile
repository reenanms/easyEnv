FROM mcr.microsoft.com/mssql/server:2017-latest

    ENV ACCEPT_EULA=Y
    ENV SA_PASSWORD=Test@12345
    ENV MSSQL_PID=Express
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