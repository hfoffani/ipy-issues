import os
basedir = 'C:\\Program Files (x86)\\IronPython 2.7\\' 
all_files = []
for dir, blah, files in os.walk(basedir + 'Lib'):
    if dir.startswith('Lib\\site-packages'):
        print('skipping', dir)
        continue
    for file in files:
        if file.endswith('.py'):
            all_files.append(dir + '\\' + file)

import clr
clr.CompileModules('z:\\Documents\\ipy-issues\\issues\\LibPy.dll', *all_files)
