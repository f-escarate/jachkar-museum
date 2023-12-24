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


    def run(self, path, name, remove_backgrounds = False):
        convert_res = os.system(f'python ./gaussian-splatting/convert.py -s {path}')
        if convert_res != 0:
            print("Error al usar COLMAP")
            return 1
        if remove_backgrounds:
            self.remove_backgrounds(path)
        train_res = os.system(f'python ./gaussian-splatting/train.py -s {path} --iterations 7000 --model_path ./output/{name}')
        if train_res != 0:
            print("Error al entrenar")
            return 2
        return 0


    def unity_import(self, gs_name):
        path = f"./crowdsourcingApp/backend/representationModel/output/{gs_name}/point_cloud/iteration_7000/point_cloud.ply"
        print("Importing gaussians into Unity")
        completed = subprocess.run([UNITY_PATH, "-quit", "-batchMode", "-logFile", ".\\logfile.log", "-projectPath", PROJECT_PATH, "-executeMethod", "AutoGSAsset.GenerateGSplatAsset", path]) 
        return_code = completed.returncode
        if return_code == 5:
            print("    -> Gaussians were not found")
            print(f"Gaussians couldn't be imported into unity")
            return
        if return_code != 0:
            print(f"A problem has occurred while importing gaussians into Unity: return code {return_code}")
            return
        print(f"Gaussians imported into unity")