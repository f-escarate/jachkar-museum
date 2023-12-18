# Backend

Para correr la etapa de obtención de representaciones 3D es necesario contar con:
* [COLMAP](https://colmap.github.io/install.html)
* CUDA (probado con v11.8)
* cudNN
* Librerías de Python (están en el requirements.txt)

### Ambiente
Se recomienda usar Anaconda, creando un ambiente que tenga las librerías especificadas en el archivo `jachkar-museum/crowdsourcingApp/backend/representationModel/requirements.txt` usando:

```conda create --name <env> --file requirements.txt```

### Uso

Los pasos para obtener una representación tridimencional, son:
* Poner el video en la carpeta: `jachkar-museum/crowdsourcingApp/backend/representationModel/videos`
* Ir a la carpeta `jachkar-museum/crowdsourcingApp/backend/representationModel`
* Correr en una terminal `python pipeline.py <nombre_video> <n_frames>`
