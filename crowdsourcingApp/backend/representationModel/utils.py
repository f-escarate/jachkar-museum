import os
from rembg import remove

def remove_background(photo, in_path, out_path):
    photo_path = os.path.join(in_path, photo)
    output_path = os.path.join(out_path, photo)
    
    with open(photo_path, 'rb') as i:
        with open(output_path, 'wb') as o:
            input = i.read()
            output = remove(input)
            o.write(output)