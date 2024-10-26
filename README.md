# 3. Crea tus propias imágenes de Docker

¡Hola developer 👋🏻! Este repo contiene las demos mostradas en el vídeo de mi canal de Youtube llamado [**3. Crea tus propias imágenes de Docker**](https://www.youtube.com/watch?v=nggu0pcLsxM).

[![3.Crea tus propias imágenes de Docker](docs/images/Capítulo%203%20Crea%20tus%20propias%20Imágenes%20de%20Docker.png)](https://www.youtube.com/watch?v=nggu0pcLsxM)

## Cómo crear un Dockerfile

1. Crea un archivo llamado `Dockerfile` en la raíz de tu proyecto.

2. En él escribe las instrucciones necesarias para construir tu imagen de Docker. En este repo hay uno de ejemplo para contenerizar la aplicación dentro del directorio `site`.

3. Ejecuta el comando `docker build --tag <nombre-de-tu-imagen> .` para construir tu imagen. Asegúrate de que el comando se ejecuta en el mismo directorio donde se encuentra el `Dockerfile`.

```bash
docker build -t starwarsapp .
```

## Cómo ver las imágenes que tienes en tu máquina

```bash
docker images
```


## Cómo ejecutar contenedores usando la imagen que has creado

```bash
docker run --name web -p 8080:80 -d starwarsapp
```


## Cómo crear diferentes versiones de tu imagen

1. Cambia el código de tu aplicación.

2. Construye una nueva imagen con un nuevo tag.

```bash
docker build -t starwarsapp:blue .
```

o si el color que hemos dado a las letras de Star Wars es el amarillo:

```bash
docker build -t starwarsapp:yellow .
```

Como vimos en el vídeo.


Y ejecutar versión concreta de tu imagen, simplemente cambia el tag en el comando `docker run`.

```bash
docker run --name starwarsyellow -p 8080:80 -d starwarsapp:yellow
```

## Cómo saber qué imagen usa cada contenedor

```bash
docker ps
```

En el resultado de este comando puedes ver la columna `IMAGE` que te indica qué imagen está usando cada contenedor.

## Cómo generar imágenes para un proyecto en .NET

Lo primero que necesitas es tener instalado el SDK de .NET Core. Puedes descargarlo desde la web oficial de .NET Core.

Para el ejemplo del vídeo usé el siguiente comando para crear una API en este lenguaje de programación:

```bash
dotnet new webapi -o tour-of-heroes-api
```

En este repo ya tienes el repo con el ejemplo.

La primera versión del Dockerfile que usé para contenerizar la aplicación fue la siguiente:

```Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

COPY publish .

ENTRYPOINT ["dotnet", "tour-of-heroes-api.dll"]
```

En este caso necesitas que la aplicación esté compilada y publicada en un directorio llamado `publish`. Para publicar la aplicación, ejecuta el siguiente comando:

```bash
dotnet publish -c Release -o publish
```

Para construir la imagen de Docker, ejecuta el siguiente comando:

```bash
docker build -t tour-of-heroes-api:v1 .
```

Y para ejecutar un contenedor con la imagen que acabas de crear, ejecuta el siguiente comando:

```bash
docker run --name tour-of-heroes-api-v1 -p 9000:5000 -d tour-of-heroes-api:v1
```

Sin embargo esto nos genera una fuerte dependencia con mi máquina local. Por lo que lo siguiente que probamos era cómo podíamos tener como parte de la imagen el SDK de .NET Core para que no tuviéramos que tenerlo instalado en nuestra máquina local.

Para ello, creamos un nuevo Dockerfile que incluía el SDK de .NET Core:

```Dockerfile
# FROM mcr.microsoft.com/dotnet/aspnet:8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

COPY . .

RUN dotnet publish "tour-of-heroes-api.csproj" -c Release -o /app

ENTRYPOINT ["dotnet", "tour-of-heroes-api.dll"]
```

Con este Dockerfile, ya no necesitas tener instalado el SDK de .NET Core en tu máquina local. Para construir la imagen de Docker, ejecuta el siguiente comando:

```bash
docker build -t tour-of-heroes-api:v2 .
```

Y para ejecutar un contenedor con la imagen que acabas de crear, ejecuta el siguiente comando:

```bash
docker run --name tour-of-heroes-api-v2 -p 9001:5000 -d tour-of-heroes-api:v2
```

El problema en este caso, frente a la primera versión, es que la imagen es más pesada. Por lo que en el vídeo vimos cómo podíamos mejorar esto usando multi-stage builds.


## Crear imágenes usando multi-stage builds

En el vídeo también vimos cómo crear imágenes de Docker usando multi-stage builds. Para ello, necesitas un archivo `Dockerfile` que contenga varias etapas. En este repo tienes un ejemplo de cómo hacerlo.

```bash
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app

EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .

RUN dotnet publish "tour-of-heroes-api.csproj" -c Release -o /app/publish

FROM base AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "tour-of-heroes-api.dll"]
```

Que es el contenido del archivo `Dockerfile` que se encuentra en el directorio `tour-of-heroes-api` de este repo.

Si generas la imagen con este Dockerfile, la imagen resultante será más ligera que la que generamos en la versión anterior.

```bash
docker build -t tour-of-heroes-api:v3 .
```

Y para ejecutar un contenedor con la imagen que acabas de crear, ejecuta el siguiente comando:

```bash
docker run --name tour-of-heroes-api-v3 -p 9002:5000 -d tour-of-heroes-api:v3
```

Si ahora haces un `docker images`, verás que la imagen que acabas de crear es más ligera que la que generamos en la versión anterior.


Si quieres saber más sobre cómo crear tus propias imágenes de Docker, te invito a ver el vídeo en el que se explica todo esto con más detalle.

¡Nos vemos 👋🏻!