import os
import cv2
from sys import argv
from gSplatModel import gSplatModel

def try_to_create_dir(path):
    try:
        os.mkdir(path)
    except FileExistsError:
        print(f"The directory {path} already exists")

def video_to_photos_ffmpeg(input, output_path, qscale=1, qmin=1, fps=2):
    # Creating the output directory
    path = os.path.join(output_path, input.split(".")[0])
    try_to_create_dir(path)
    imgs_path = os.path.join(path, "input")
    try_to_create_dir(imgs_path)
    os.system(f"ffmpeg -i videos\{input} -qscale:v {qscale} -qmin {qmin} -vf fps={fps} {imgs_path}\%04d.jpg")
    return path

def video_to_photos(input, n_frames, output_path):
    # Creating the output directory
    path = os.path.join(output_path, input.split(".")[0]+f"_{n_frames}frames")
    try_to_create_dir(path)
    imgs_path = os.path.join(path, "input")
    try_to_create_dir(imgs_path)
    
    # Open the video
    vidcap = cv2.VideoCapture(f"videos\{input}")
    # Get total number of frames
    total_frames = int(vidcap.get(cv2.CAP_PROP_FRAME_COUNT))
    # Calculate spacing between frames
    frame_spacing = max(total_frames // n_frames, 1)
    
    # Iterate through the video frames
    count = 0
    success = True
    while count < total_frames and success:
        vidcap.set(cv2.CAP_PROP_POS_FRAMES, count) # Set frame position
        success, image = vidcap.read()
        if count % frame_spacing == 0:
            # Save the frame as an image file
            frame_filename = os.path.join(imgs_path, f"frame_{count:04d}.jpg")
            cv2.imwrite(frame_filename, image)
        
        count += frame_spacing

    vidcap.release()
    return path

def create_gsplat(video, n_frames=45):    
    OUTPUT_PATH = ".\gaussian-splatting\data"
    name = f"{video.split('.')[0]}_{n_frames}frames"
    #path = video_to_photos_ffmpeg(video, OUTPUT_PATH)
    path = video_to_photos(video, n_frames, OUTPUT_PATH)
    model = gSplatModel()
    run_res = model.run(path, name)
    if run_res == 0:
        model.unity_import(name)

if __name__ == "__main__":
    create_gsplat(argv[1], int(argv[2]))
