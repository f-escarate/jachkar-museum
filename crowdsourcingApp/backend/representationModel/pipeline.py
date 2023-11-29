import os
from sys import argv
from gSplatModel import gSplatModel

def try_to_create_dir(path):
    try:
        os.mkdir(path)
    except FileExistsError:
        print(f"The directory {path} already exists")

def video_to_photos(input, qscale, qmin, fps, output_path):
    # Creating the output directory
    path = os.path.join(output_path, input.split(".")[0])
    try_to_create_dir(path)
    imgs_path = os.path.join(path, "input")
    try_to_create_dir(imgs_path)
    os.system(f"ffmpeg -i videos\{input} -qscale:v {qscale} -qmin {qmin} -vf fps={fps} {imgs_path}\%04d.jpg")
    return path

def create_gsplat(video, qscale, qmin, fps):    
    OUTPUT_PATH = ".\gaussian-splatting\data"
    path = video_to_photos(video, qscale, qmin, fps, OUTPUT_PATH)
    model = gSplatModel()
    model.run(path)

if __name__ == "__main__":
    #create_gsplat(argv[1], argv[2], argv[3], argv[4])
    create_gsplat(argv[1], 1, 1, 2)
