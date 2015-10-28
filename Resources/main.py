# -*- coding: utf8 -*-

import os
import fnmatch
import threading
import sys
import multiprocessing
import time
from ConvertPdfToPng import *
from CageManager.CageManager import *
from GoManager.GoManager import *
from HasSheetsGenerator.HasSheetsGenerator import *
from LogHasSheetsGenerator.LogHasSheetsGenerator import *


def ConvertZoningFolder(dir, certified=True):
    ConvertPdfToPng(128, certified=certified).ConvertFolder(dir, "Niveaux")
    ConvertPdfToPng(64).ConvertFolder(dir, "Zones")

def ConvertExportFolder(dir, certified=True):
    cagesPath =  os.path.join(dir, "Cages")
    if os.path.exists(cagesPath):
        for cage in os.listdir(cagesPath):
            cagePath = "%s/%s" % (cagesPath, cage)
            if os.path.exists("%s/LocationPlan.pdf" % cagePath):
                ConvertPdfToPng(64, certified=False).GeneratePngFromFile("%s/LocationPlan.pdf" % cagePath)
            ConvertZoningFolder(cagePath, certified=False)
        CageManager(dir).Run()

    sheetsPath = os.path.join(dir, "Fiches")
    if os.path.exists(sheetsPath):
        ConvertExportFolder(sheetsPath, False)
        GoManager(dir).Run()

    sheetsPath = os.path.join(dir, "Views")
    if os.path.exists(sheetsPath):
        ConvertExportFolder(sheetsPath, False)
        HasSheetsGenerator(dir).Run()

    sheetsPath = os.path.join(dir, "LogViews")
    if os.path.exists(sheetsPath):
        ConvertExportFolder(sheetsPath, False)
        LogHasSheetsGenerator(dir).Run()
    ConvertZoningFolder(dir)

def LaunchScript(scriptPath, projectsFolder):
    convertPath = scriptPath + "/../"

    for file in os.listdir(convertPath):
        projectName, fileExtension = os.path.splitext(file)
        if (fileExtension == ".convertMe"):
            dirs = []

            f = open("%s/%s" % (convertPath, file))
            for line in f:
                folderExportName = line.strip()
                dirs.append("%s/%s/%s" % (projectsFolder, projectName, folderExportName))

            f.close()
            os.remove("%s/%s" % (convertPath, file))
            for dir in dirs:
                ConvertExportFolder(dir)
                renameExportFolder(dir)

    while (threading.active_count() > 1):
        time.sleep(1)

def renameExportFolder(folderExportPath):
    while (threading.active_count() > 1):
        threading._sleep(1)
    if folderExportPath[len(folderExportPath)-1] == "/" or folderExportPath[len(folderExportPath)-1] == "\\":
        folderExportPath = folderExportPath[:len(folderExportPath)-1]
    if (verifPNG(os.path.join(folderExportPath, "Zones")) and verifPNG(os.path.join(folderExportPath, "Niveaux"))):
        os.rename(folderExportPath, folderExportPath + " PNG OK")
    else:
        os.rename(folderExportPath, folderExportPath + " PNG ERROR")


def verifPNG(folderExportPath):
    for root, dirnames, filenames in os.walk(folderExportPath):
        for filename in fnmatch.filter(filenames, '*.pdf'):
            filePath = os.path.join(root, filename)
            if not os.path.exists(filePath.replace('.pdf', '.png')):
                return False
    return True

if __name__ == "__main__":
    if len(sys.argv) == 2:
        dir = sys.argv[1]
        ConvertExportFolder(dir)
    elif len(sys.argv) == 3:
        dir = sys.argv[1]
        color = int (sys.argv[2])
        ConvertPdfToPng(color).ConvertFolder(dir, "")

    else:
        scriptPath = os.path.dirname(os.path.realpath(__file__))
        projectsFolder = "/HD1-4TO/production/1_Production/1_En_cours/"

        if not os.path.exists(os.path.join(scriptPath,"..", ".tmpConvertPng")):
            with open(os.path.join(scriptPath,"..", ".tmpConvertPng"), 'a'):
                try: LaunchScript(scriptPath, projectsFolder)
                except: None
            os.remove(os.path.join(scriptPath,"..", ".tmpConvertPng"))
