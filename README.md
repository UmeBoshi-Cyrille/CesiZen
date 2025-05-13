# API CesiZen - Guide d'installation et de configuration

Voici un guide complet pour l'installation et la configuration de l'API CesiZen construite avec ASP.NET Core, PostgreSQL et Docker.

## Prérequis

- Docker Desktop installé (Windows/Mac)
- .NET SDK 8.0
- Git (pour cloner le dépôt)

## Installation

Installation du projet : 
git clone [URL_DU_DÉPÔT]  

_
#### Configuration Docker
Fichier Docker Compose (docker-compose.yml)

```
# docker-compose-db.yml
version: '3.8'

services:
  database:
    image: postgres:17.2-alpine
    environment:
      POSTGRES_USER: [IDENTIFIANT]
      POSTGRES_PASSWORD: [MOT DE PASSE]
      POSTGRES_DB: cesizendb
    volumes:
      - postgres_data:/var/lib/postgresql/data
    ports:
      - "5432:5432"     

volumes:
  postgres_data:
    driver: local
```
_
#### Construction et lancement des conteneurs
Ouvrir le fichier docker-compose dans un terminal et lancer la commande suivante : 
```
docker-compose up
```
Vérifier que votre container est bien actif en utilisant la commande :

```
docker ps
```

_

#### Migrations
Ouvrez le projets de l'api dans votre IDE, ajouter une chaine de connexion dans le fichier appSettings-development.json, dans la section suivante : 

```
"ConnectionStrings": {
  "Postgres": "Server=localhost;Database=cesizendb;TrustServerCertificate=True;UserID=votre-identifiant;Password=votre-mot-de-passe"
},
```
Appliquez ensuite les migrations à la base de données en utilisant la commande suivante dans le terminal 
```
dotnet ef database update
```
Cette commande peut également être exécutée via la Package Manager Console dans Visual Studio avec :

```
Update-Database
```

_

#### Lancement du projet

```
dotnet build
```

```
dotnet run
```

## Usage

Une fois démarré, une fenêtre devrait s'ouvrir automatiquement dans votre navigateur.  

Si ce n'est pas le cas, vous pouvez utiliser ce lien : 

 >https://localhost:7103/swagger/index.html

Vous pourrez ainsi tester à votre guise les fonctionnalités présente.

## License

[MIT](https://choosealicense.com/licenses/mit/)