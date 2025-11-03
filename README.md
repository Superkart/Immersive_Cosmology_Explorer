# Immersive Cosmology Explorer (Unity + VR)
A Unity 2022.3.62f3 (LTS) project for immersive visualization and collaborative exploration of cosmological datasets in VR and on desktop.

## 1. Prerequisites

Unity Hub and Unity 2022.3.62f3 (LTS)
Git (version 2.4 or later)
Git LFS (Large File Storage)
Optional: VR headset (Meta Quest, HTC Vive) with OpenXR runtime

## 2. Repository Setup
If cloning an existing repository:
git clone <YOUR_REPO_URL>
cd <repo-folder>
git lfs install

If starting a new repository:
mkdir <repo-folder>
cd <repo-folder>
git init
curl -L https://raw.githubusercontent.com/github/gitignore/main/Unity.gitignore -o .gitignore
git add .gitignore
git commit -m "Add Unity .gitignore"

## 3. Open the Project in Unity
Open Unity Hub and click "Add Project".
Select the cloned or created folder.
Once opened, go to Edit → Project Settings → Editor:
Set "Version Control" to "Visible Meta Files".
Set "Asset Serialization" to "Force Text".
Save your main scene as Assets/Scenes/Main.unity.

## 4. XR / VR Setup
Go to Window → Package Manager and install:
OpenXR Plugin (1.9 or later)
XR Interaction Toolkit (version 3 or later)
XR Management
Open Edit → Project Settings → XR Plug-in Management and enable OpenXR for your platform.
Under OpenXR → Features, enable the interaction profiles you need (Meta, HTC, etc.).

Commit your changes:
git add .
git commit -m "Configure XR packages and settings"

## 5. Basic Git Usage
git status
git add .
git commit -m "Your message"
git push
git pull --rebase

## 6. Git LFS Setup
Install and configure Git LFS for large binary files:

git lfs install
git lfs track "*.fbx" "*.wav" "*.mp4" "*.tif" "*.exr" "*.zip"
git add .gitattributes
git commit -m "Add Git LFS tracking"
git push

## 7. Running the Project
Open the project in Unity 2022.3.62f3.
Open the scene Assets/Scenes/Main.unity.
Press Play to test in the editor, or build to your VR headset from File → Build Settings.

## 8. Troubleshooting
If Unity packages are missing, delete the "Library" folder and reopen the project.
If large files are missing, run git lfs pull.
If VR is not detected, check your active OpenXR runtime (SteamVR, Oculus PC, etc.).
