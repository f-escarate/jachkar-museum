import subprocess
import os
from utils import remove_background

class gSplatModel():
    
    def remove_backgrounds(self, path):
        photos = os.listdir(os.path.join(path, "input"))
        cleaned_path = os.path.join(path, "images")
        os.mkdir(cleaned_path)
        for photo in photos:
            remove_background(photo, path, cleaned_path)


    def run(self, path, remove_backgrounds = False):
        subprocess.run(["python", "./gaussian-splatting/convert.py", "-s", path])
        if remove_backgrounds:
            self.remove_backgrounds(path)
        subprocess.run(["python", "./gaussian-splatting/train.py", "-s", path, "--iterations", ["7_000"]]) 