import subprocess
import os
from utils import remove_background

UNITY_PATH = r"D:\Programas\Unity Hub\Editor\2022.3.5f1\Editor\Unity.exe"
PROJECT_PATH = "..\\..\\..\\"

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
    
    def unity_import(self, path):
        subprocess.run([UNITY_PATH, "-quit", "-batchMode", "-projectPath", PROJECT_PATH, "-executeMethod", "AutoGSAsset.GenerateGSplatAsset", "-logFile", ".\\logfile.log"]) 
